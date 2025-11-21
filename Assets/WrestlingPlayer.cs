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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

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
            Destroy(other.gameObject);
            Time.timeScale = 0;
            //win
        }
        else if(other.gameObject.tag == "Ground")
        {
            Debug.Log("hit ground");
            Time.timeScale = 0;
        }
    }
}
