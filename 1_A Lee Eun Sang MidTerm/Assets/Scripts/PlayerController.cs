using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }


    private void Update()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;

        if (moveInput == 0)
        {
            GetComponent<Animator>().SetBool("IsMove", false);
        }
        else if (moveInput < 0)
        {
            GetComponent<Animator>().SetBool("IsMove", true);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (moveInput > 0)
        {
            GetComponent<Animator>().SetBool("IsMove", true);
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocityY > 0)
        {
            GetComponent<Animator>().SetBool("IsUp", true);

        }
        else if (rb.linearVelocityY < 0)
        {
            GetComponent<Animator>().SetBool("IsUp", false);
        }
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            GetComponent<Animator>().SetBool("OnAir", !isGrounded);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
        GetComponent<Animator>().SetBool("OnAir", !isGrounded);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        GetComponent<Animator>().SetTrigger("Land");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        GetComponent<Animator>().SetBool("OnAir", !isGrounded);
    }
    
}


