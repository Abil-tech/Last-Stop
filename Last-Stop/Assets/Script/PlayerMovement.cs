using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float moveInput;
    private Camera mainCamera;
    private float objectWidth;

    // --- DECLARATION CONTEXT EXAMPLE ---
    // We declare an AudioSource specific to the player, along with step cadence variables
    // to prevent the audio from overlapping and sounding completely cooked.
    [Header("Audio Settings")]
    public AudioSource footstepSource; 
    public AudioClip footstepClip;     
    public float stepInterval = 0.4f;   // Lower = faster steps (running), Higher = slower steps
    private float stepTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        
        if (GetComponent<SpriteRenderer>() != null)
        {
            objectWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        }
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x > Screen.width / 2)
                moveInput = 1f;
            else
                moveInput = -1f;
        }

        // Handle footstep cadence timing logic
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        // Only tick down the timer if the player is actively moving left or right
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                if (footstepSource != null && footstepClip != null)
                {
                    footstepSource.PlayOneShot(footstepClip);
                }
                stepTimer = stepInterval; // Reset the cadence window
            }
        }
        else
        {
            // Reset the timer so footsteps play instantly the microsecond they start walking again
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