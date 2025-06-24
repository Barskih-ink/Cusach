using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce = 8f;

    private Rigidbody2D rb;
    private PlayerWallGrab wallGrab;
    private PlayerMovement movement;
    private PlayerHealth health;

    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallGrab = GetComponent<PlayerWallGrab>();
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (health != null && health.isDead) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (wallGrab != null && wallGrab.IsGrabbingWall) 
            {
                
                float wallJumpDirection = movement.MoveSpeed > 0 ? -1 : 1;
                rb.linearVelocity = new Vector2(wallJumpDirection * movement.MoveSpeed * 1.2f, jumpForce);

                wallGrab.StopWallGrab(); 
            }
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

}
