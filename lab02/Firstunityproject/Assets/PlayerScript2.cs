using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript2 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 7f;
    Rigidbody2D rb;
    SpriteRenderer sr;
    bool grounded, jumpQueued;
    float move;

    InputAction moveAction, jumpAction;

    void Awake()
    {
        Debug.Log("started");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.freezeRotation = true;

        moveAction = new InputAction("Move", InputActionType.Value);

        var axis = moveAction.AddCompositeBinding("1DAxis");
        axis.With("negative", "<Keyboard>/a").With("negative", "<Keyboard>/leftArrow");
        axis.With("positive", "<Keyboard>/d").With("positive", "<Keyboard>/rightArrow");

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        move = Mathf.Clamp(moveAction.ReadValue<float>(), -1f, 1f);
        if (jumpAction.WasPressedThisFrame())
            jumpQueued = true;
        if (sr && move != 0)
            sr.flipX = move < 0;
    }

    void FixedUpdate()
    {
        var lv = rb.linearVelocity;
        lv.x = move * moveSpeed;
        rb.linearVelocity = lv;

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
