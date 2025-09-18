using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript2 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;   // base speed fallback
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PowerUpController powerUpCtrl;

    private bool grounded;
    private bool jumpQueued;
    private float move;

    // Input System actions
    private InputAction moveAction;
    private InputAction jumpAction;

    void Awake()
    {
        Debug.Log("Player started");

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        powerUpCtrl = GetComponent<PowerUpController>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.freezeRotation = true;

        // Set up movement input (A/D or arrows)
        moveAction = new InputAction("Move", InputActionType.Value);
        var axis = moveAction.AddCompositeBinding("1DAxis");
        axis.With("negative", "<Keyboard>/a").With("negative", "<Keyboard>/leftArrow");
        axis.With("positive", "<Keyboard>/d").With("positive", "<Keyboard>/rightArrow");

        // Set up jump input (space)
        jumpAction = new InputAction("Jump", InputActionType.Button);
        jumpAction.AddBinding("<Keyboard>/space");
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Update()
    {
        // Use PowerUp speed if available, else fallback to base moveSpeed
        float speed = powerUpCtrl ? powerUpCtrl.CurrentMoveSpeed : moveSpeed;

        // Read horizontal movement (-1 to 1)
        move = Mathf.Clamp(moveAction.ReadValue<float>(), -1f, 1f);

        // Queue jump if pressed
        if (jumpAction.WasPressedThisFrame())
            jumpQueued = true;

        // Flip sprite to face move direction
        if (sr && move != 0)
            sr.flipX = move < 0;
    }

    void FixedUpdate()
    {
        // Apply horizontal movement
        float speed = powerUpCtrl ? powerUpCtrl.CurrentMoveSpeed : moveSpeed;
        var lv = rb.linearVelocity;
        lv.x = move * speed;
        rb.linearVelocity = lv;

        // Handle jumping
        if (jumpQueued && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        jumpQueued = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            grounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            grounded = false;
    }
}
