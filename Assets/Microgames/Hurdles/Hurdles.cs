using UnityEngine;
using Helper.Blake;

public class Hurdles : MonoBehaviour
{
    public float speed = 5f;
    public float stopX = 10f;
    public float jumpForce = 7f;
    public bool Jumping = false;

    private Rigidbody2D rb2d;
    private Animator anim;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // or Rigidbody2D if 2D
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (transform.position.x < stopX)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            // Subscribe to input event
            EventManager.Instance.correctKeyInput += OnCorrectKeyPressed;
        }
    }


    private void OnCorrectKeyPressed(string key)
    {
        Jump();
    }

    private void Jump()
    {
        // Reset vertical velocity to make jumps consistent
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);

        // Add jump force
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Play jump animation
        anim.SetBool("Jumping", true);

    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("Jumping", false);
        }
        if (collision.gameObject.CompareTag("Hurdle"))
        {
            Debug.Log("YOU LOSE!");
            // Add lose logic here
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("YOU WIN!");
            // Add win logic here
        }
    }
}