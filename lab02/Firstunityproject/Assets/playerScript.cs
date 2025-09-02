using UnityEngine;

public class playerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool doubleJump; 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        if (isGrounded)
        {
           rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocityY);  
        }
        if (move > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        } else if (move < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetButtonDown("Jump") && isGrounded || Input.GetButtonDown("Jump") && doubleJump == true)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);   
                doubleJump = true;
            }
            else if (doubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
                doubleJump = false;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJump = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}