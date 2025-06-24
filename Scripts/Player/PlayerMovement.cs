using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;

    private float moveInput;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerWallGrab wallGrab;
    private PlayerHealth health;
    private Animator animator;

    private bool isRolling = false;
    public float rollDuration = 0.5f;
    private float rollTimer = 0f;

    public bool IsRolling => isRolling;

    [Header("Footstep Sound")]
    public AudioClip footstepSound;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallGrab = GetComponent<PlayerWallGrab>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (health != null && health.isDead) return;

        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                EndRoll();
            }
            StopFootsteps(); // не проигрывать звук в перекате
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        // Перекат
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            bool isGrounded = GetComponent<PlayerJump>().IsGrounded();
            float speed = Mathf.Abs(rb.linearVelocity.x);

            if (isGrounded && speed >= 0)
            {
                StartRoll();
                StopFootsteps(); // не проигрывать звук в перекате
                return;
            }
        }

        // Спрайт поворот
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        HandleFootstepSound(); // <<< новый метод
    }

    private void FixedUpdate()
    {
        if (health != null && health.isDead) return;

        if (isRolling)
        {
            float rollSpeed = moveInput != 0 ? Mathf.Sign(moveInput) * MoveSpeed * 1.5f : (spriteRenderer.flipX ? -1 : 1) * MoveSpeed * 1.5f;
            rb.linearVelocity = new Vector2(rollSpeed, rb.linearVelocity.y);
            return;
        }

        if (wallGrab != null && wallGrab.IsGrabbingWall)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * MoveSpeed, rb.linearVelocity.y);
    }

    private void StartRoll()
    {
        isRolling = true;
        rollTimer = rollDuration;
        animator.SetBool("isRolling", true);
    }

    private void EndRoll()
    {
        isRolling = false;
        animator.SetBool("isRolling", false);
    }

    private void HandleFootstepSound()
    {
        bool isMoving = Mathf.Abs(moveInput) > 0.1f;
        bool isGrounded = GetComponent<PlayerJump>().IsGrounded();

        if (isMoving && isGrounded && !isRolling && !audioSource.isPlaying)
        {
            if (footstepSound != null)
                audioSource.clip = footstepSound;

            audioSource.loop = true;
            audioSource.Play();
        }
        else if ((!isMoving || !isGrounded || isRolling) && audioSource.isPlaying)
        {
            StopFootsteps();
        }
    }

    private void StopFootsteps()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
