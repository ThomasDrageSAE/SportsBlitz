using UnityEngine;
using Helper.Blake;
using SportsBlitz.Events;

public class Hurdles : MonoBehaviour
{
    public float speed = 5f;
    public float stopX = 10f;
    public float jumpForce = 7f;
    public bool Jumping = false;
    public bool canMove = true;

    private Rigidbody2D rb2d;
    private Animator anim;

    private MinigameManager minigame;

    public AudioClip jumpSfx;


    void Start()
    {
        minigame = FindAnyObjectByType<MinigameManager>();
        rb2d = GetComponent<Rigidbody2D>(); // or Rigidbody2D if 2D
        anim = GetComponent<Animator>();
       
        // Subscribe to input event
        EventManager.Instance.correctKeyInput += OnCorrectKeyPressed;
    }

    void Update()
    {
        if (canMove && transform.position.x < stopX)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        }
    }


    private void OnCorrectKeyPressed(string key)
    {
        if (Jumping == false)
        {
            Jump();
        }
        else
        {
            return;
        }
    }

    private void Jump()
    {
        if (!canMove)
        {
            return;
        }

        // Reset vertical velocity to make jumps consistent
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);

        // Add jump force
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Play jump animation
        anim.SetBool("Jumping", true);
        if (jumpSfx != null) AudioSource.PlayClipAtPoint(jumpSfx, Vector3.zero, 1.0f);



    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("Jumping", false);
        }
        if (collision.gameObject.CompareTag("Hurdle"))
        {
            anim.SetBool("Fall", true);
            canMove = false;
            anim.SetBool("Jumping", false);
            rb2d.linearVelocity = Vector2.zero;
            Debug.Log("YOU LOSE!");
            minigame.Lose();
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("YOU WIN!");
            minigame.Win();
        }
    }
}