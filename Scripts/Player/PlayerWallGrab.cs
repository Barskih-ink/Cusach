using UnityEngine;

public class PlayerWallGrab : MonoBehaviour
{
    [Header("Wall Grab Settings")]
    public Transform[] wallCheckPoints;
    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;
    public bool isGrabbingWall = false;

    private Rigidbody2D rb;

    public bool IsGrabbingWall => isGrabbingWall;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsTouchingWall())
        {
            isGrabbingWall = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }

        if (isGrabbingWall && (!IsTouchingWall() || IsGrounded()))
        {
            StopWallGrab();
        }
    }

    private bool IsTouchingWall()
    {
        foreach (var point in wallCheckPoints)
        {
            RaycastHit2D hitRight = Physics2D.Raycast(point.position, Vector2.right, wallCheckDistance, wallLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(point.position, Vector2.left, wallCheckDistance, wallLayer);

            Debug.DrawRay(point.position, Vector2.right * wallCheckDistance, Color.red);
            Debug.DrawRay(point.position, Vector2.left * wallCheckDistance, Color.blue);

            if (hitRight.collider != null || hitLeft.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsGrounded()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, wallLayer);
        return groundHit.collider != null;
    }

    public void StopWallGrab()
    {
        isGrabbingWall = false;
        rb.gravityScale = 1f;
    }
}
