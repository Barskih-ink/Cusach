using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public float gravityScale = 3f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyCustomGravity()
    {
        rb.linearVelocity += Vector2.down * gravityScale * Time.deltaTime;
    }
}
