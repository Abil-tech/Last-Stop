using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float moveInput;
    private Camera mainCamera;
    private float objectWidth;

    [Header("Audio Settings")]
    public AudioSource footstepSource; 
    public AudioClip footstepClip;     
    public float stepInterval = 0.4f;   
    private float stepTimer;

    private Animator anim;
    private bool facingRight = true; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        anim = GetComponent<Animator>();

        if (GetComponent<SpriteRenderer>() != null)
        {
            objectWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        }
    }

    void Update()
    {
        // KODE MODIFIKASI:
        // Jika tidak ada input dari tombol UI (nilainya 0), 
        // kita tetap izinkan input dari Keyboard/Arrow Key biar gampang testing.
        if (moveInput == 0)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }

        // Animasi
        if (anim != null)
        {
            bool isMoving = Mathf.Abs(moveInput) > 0.1f;
            anim.SetBool("isWalking", isMoving);
        }

        // Flip Karakter
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        HandleFootsteps();
    }

    // --- FUNGSI BARU UNTUK TOMBOL UI ---
    // Fungsi ini akan dipanggil saat tombol ditekan atau dilepas
    public void SetMoveInput(float input)
    {
        moveInput = input;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    void HandleFootsteps()
    {
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                if (footstepSource != null && footstepClip != null)
                {
                    footstepSource.PlayOneShot(footstepClip);
                }
                stepTimer = stepInterval; 
            }
        }
        else
        {
            stepTimer = 0f; 
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        float minX = -screenBounds.x + objectWidth;
        float maxX = screenBounds.x - objectWidth;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}