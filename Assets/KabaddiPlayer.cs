using UnityEngine;

public class KabaddiPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    private float movementSpeed;
    public KabaddiManager gameManager;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStart == false)
        {
            movementSpeed = 0;
        }
        else
        {
            movementSpeed = speed * 100;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Vector2 position = transform.position;
            position.y += movementSpeed * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector2 position = transform.position;
            position.y -= movementSpeed * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector2 position = transform.position;
            position.x += movementSpeed * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector2 position = transform.position;
            position.x -= movementSpeed * Time.deltaTime;
            transform.position = position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Finish")
        {
            gameManager.Win();
        }
    }

}
