using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private PlayerJump jump;
    private PlayerLadderClimb ladderClimb;
    private PlayerWallGrab wallGrab;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        jump = GetComponent<PlayerJump>();
        ladderClimb = GetComponent<PlayerLadderClimb>();
        wallGrab = GetComponent<PlayerWallGrab>();
    }

    void Update()
    {
        // �������� ��� ������/����
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        // ������
        bool isJumping = rb.linearVelocity.y > 0.1f || (!jump.IsGrounded() && rb.linearVelocity.y < -0.1f);
        animator.SetBool("isJumping", isJumping && !ladderClimb.IsClimbing());

        // ��������
        animator.SetBool("isClimbing", ladderClimb.IsClimbing());

        // ����� (hanging)
        animator.SetBool("isHanging", wallGrab.IsGrabbingWall && !jump.IsGrounded());
    }
}
