using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float moveInput;
    private Camera mainCamera;
    private float objectWidth;

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
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        float minX = -screenBounds.x + objectWidth;
        float maxX = screenBounds.x - objectWidth;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector2(clampedX, transform.position.y);
    }
}