using UnityEngine;

public class KabaddiOpponent : MonoBehaviour
{
    [SerializeField] KabaddiManager gameManager;
    [SerializeField] GameObject target;
    Vector2 targetPosition;
    [SerializeField] float speed;
    float movementSpeed;

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameStart == false)
        {
            movementSpeed = 0;
        }
        else
        {
            movementSpeed = speed * 100 * Time.deltaTime;
        }

        targetPosition = target.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            gameManager.Lose();
        }
    }
}
