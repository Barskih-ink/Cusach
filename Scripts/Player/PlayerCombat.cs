using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerWallGrab wallGrab;
    public Transform attackPoint;
    public float attackRange = 0.3f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    public Animator animator;

    private SpriteRenderer spriteRenderer;
    private PlayerHealth health;

    private bool isAttacking = false;
    public float attackCooldown = 0.25f;

    [Header("Sound Effects")]
    public AudioClip hitSound;
    public AudioClip swingSound;
    public AudioSource sfxSource; // ��������� AudioSource ��� ������

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wallGrab = GetComponent<PlayerWallGrab>();
        animator = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();

        if (sfxSource == null)
            Debug.LogWarning("sfxSource (�����) �� �������� � PlayerCombat.");
    }

    void Update()
    {
        if (health != null && health.isDead) return;

        if (Input.GetMouseButtonDown(0) && !isAttacking && !wallGrab.IsGrabbingWall)
        {
            animator.Play("attacks", 0, 0);
            isAttacking = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }

        FlipAttackPoint();
    }

    void ResetAttack() => isAttacking = false;

    public void PerformAttack()
    {
        Attack();
    }

    void Attack()
    {
        // ������ ������ ���� ������
        if (sfxSource != null && swingSound != null)
            sfxSource.PlayOneShot(swingSound);

        // ��������� ���������
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(attackDamage);

            // ������ ���� ��������� �� �����
            if (sfxSource != null && hitSound != null)
                sfxSource.PlayOneShot(hitSound);
        }
    }


    void FlipAttackPoint()
    {
        if (spriteRenderer != null && attackPoint != null)
        {
            Vector3 localPos = attackPoint.localPosition;
            localPos.x = Mathf.Abs(localPos.x) * (spriteRenderer.flipX ? -1 : 1);
            attackPoint.localPosition = localPos;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
