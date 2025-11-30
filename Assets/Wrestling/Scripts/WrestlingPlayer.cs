using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class WrestlingPlayer : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed;
    public float movementSpeed;

    [SerializeField] GameObject throwingPoint;
    Vector2 startPosition;
    Vector2 throwingPosition;
    Vector2 direction;

    [SerializeField] private Animator playerAnimation;

    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] AudioSource floorAudioSource;

    private MinigameManager minigameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        minigameManager = FindObjectOfType<MinigameManager>();

        startPosition = rb.position;
        movementSpeed = speed * 1000;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayerJump();
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            SceneManager.LoadScene("Professional wrestling");
        }*/
    }

   public void PlayerJump()
    {
        playerAnimation.Play("Jump", 0, 0.0f);
        playerAudioSource.Play();
        throwingPosition = throwingPoint.transform.position;
        direction = throwingPosition - startPosition;
        direction.Normalize();
        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.AddForce(direction * movementSpeed);

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Table")
        {
            Debug.Log("hit table");
            enemyAudioSource.Play();
            minigameManager.Win();
            //win
        }
        else if(other.gameObject.tag == "Ground")
        {
            Debug.Log("hit ground");
            floorAudioSource.Play();
            minigameManager.Lose();
        }
    }
}
