using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isMovingRight = true;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (isMovingRight)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }

        if (rb.linearVelocity.x > 0)
        {
            sr.flipX = false;
        }
        else if (rb.linearVelocity.x < 0)
        {
            sr.flipX = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            isMovingRight = !isMovingRight;
        }
    }
}
