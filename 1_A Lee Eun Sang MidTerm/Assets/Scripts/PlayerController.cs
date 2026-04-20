using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    private float originalSpeed;
    public float jumpForce = 5f;
    private float originalJumpForce;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator pAni;
    private Animator run;
    private bool isGrounded;
    private float moveInput;

    private bool isGiant = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        run = GetComponent<Animator>();
        run.SetBool("Move", false);

        originalSpeed = moveSpeed;
        originalJumpForce = jumpForce;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
    }


    void Update()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (isGiant)
        {
            if (moveInput > 0)
            {
                transform.localScale = new Vector3(0.24f, 0.24f, 1);
            }
            else if (moveInput < 0)
            {
                transform.localScale = new Vector3(-0.24f, 0.24f, 1);
            }
        }
        else
        {
            if (moveInput > 0)
            {
                transform.localScale = new Vector3(0.12f, 0.12f, 1);
            }
            else if (moveInput < 0)
            {
                transform.localScale = new Vector3(-0.12f, 0.12f, 1);
            }
        }



        if (moveInput > 0)
        {
            run.SetBool("Move", true);
        }
        else if (moveInput < 0)
        {
            run.SetBool("Move", true);
        }
        else if (moveInput == 0)
        {
            run.SetBool("Move", false);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        pAni.SetBool("Jump", !isGrounded);
    }


    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;
    }


    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        run.SetBool("Move", Mathf.Abs(moveInput) > 0.1f && isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            if (!isGiant)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
        else if (collision.CompareTag("Goal"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            if (isGiant)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (collision.CompareTag("Item"))
        {
            isGiant = true;
            Invoke(nameof(ResetGiant), 5f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Speed Item"))
        {
            moveSpeed *= 1.5f;
            Invoke(nameof(ResetSpeed), 5f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Jump Item"))
        {
            jumpForce = originalJumpForce * 1.5f;
            CancelInvoke(nameof(ResetJump));
            Invoke(nameof(ResetJump), 5f);

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("EnemyDestroy"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }

            Destroy(collision.gameObject);
        }
    }

    void ResetSpeed()
    {
        moveSpeed = originalSpeed;
    }
    void ResetGiant()
    {
        isGiant = false;
    }

    void ResetJump()
    {
        jumpForce = originalJumpForce;
    }
}


