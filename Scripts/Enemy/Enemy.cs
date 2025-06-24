using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Основные параметры")]
    public int maxHealth = 3;
    protected int currentHealth;

    [Header("Патрулирование")]
    public Transform[] patrolPoints;   // точки патруля
    public float moveSpeed = 2f;
    private int currentPointIndex = 0;

    [Header("Обнаружение игрока")]
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    protected Transform player;

    [Header("Атака")]
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float attackCooldown = 1.5f;
    protected float lastAttackTime;

    [Header("Щит")]
    protected bool isShieldActive = false;
    protected float shieldBlockPercentage = 0f;  // Процент блокируемого урона
    protected float shieldDuration = 2f;         // Длительность действия щита
    protected float shieldCooldown = 5f;         // Время между активациями
    protected float lastShieldTime = -Mathf.Infinity;

    [Header("Souls Reward")]
    public int soulsReward = 10;

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;


    protected virtual void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        player = null;
        lastAttackTime = -attackCooldown;
    }

    protected virtual void Update()
    {
        animator.SetBool("IsWalking", rb.linearVelocity.x != 0 && player == null);
        DetectPlayer();

        if (player != null)
        {
            TryActivateShield();
            ChaseAndAttackPlayer();
        }
        else
        {
            Patrol();
        }

        if (isShieldActive && Time.time - lastShieldTime > shieldDuration)
        {
            isShieldActive = false;
            Debug.Log($"{gameObject.name} деактивировал щит.");
        }
    }

    protected virtual void TryActivateShield()
    {
        if (!isShieldActive && Time.time - lastShieldTime > shieldCooldown)
        {
            float chance = Random.value;
            if (chance < 0.01f)  // 1% шанс в кадр при возможности
            {
                ActivateShield();
            }
        }
    }

    protected virtual void ActivateShield()
    {
        isShieldActive = true;
        lastShieldTime = Time.time;
        Debug.Log($"{gameObject.name} активировал щит!");
        animator.SetTrigger("Shield");
    }

    protected void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // Флип спрайта по направлению движения
        if (direction.x > 0)
            spriteRenderer.flipX = false;
        else if (direction.x < 0)
            spriteRenderer.flipX = true;

        // Проверка близости к точке патруля
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    protected void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (playerCollider != null)
        {
            player = playerCollider.transform;
        }
        else
        {
            player = null;
        }
    }

    protected void ChaseAndAttackPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            animator.SetBool("IsWalking", rb.linearVelocity.x != 0);
            // Движемся к игроку
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

            spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            // В пределах атаки — остановиться и атаковать
            rb.linearVelocity = Vector2.zero;

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    protected virtual void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void PerformAttack()
    {
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log($"{gameObject.name} нанес урон {attackDamage} игроку");
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (isShieldActive)
        {
            int reducedDamage = Mathf.CeilToInt(damage * (1f - shieldBlockPercentage));
            Debug.Log($"{gameObject.name} заблокировал часть урона щитом: {damage} -> {reducedDamage}");
            damage = reducedDamage;
        }

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} получил урон {damage}. Остаток здоровья: {currentHealth}");
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Найти игрока и начислить души
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.AddSouls(soulsReward);
            Debug.Log($"{gameObject.name} умер. Игрок получил {soulsReward} душ.");
        }

        Destroy(gameObject, 3f); // подождать анимацию
    }

    private void OnDrawGizmosSelected()
    {
        // Показываем зону обнаружения игрока
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Показываем зону атаки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Показать точки патруля
        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in patrolPoints)
            {
                if (point != null)
                    Gizmos.DrawSphere(point.position, 0.1f);
            }
        }
    }
}