using UnityEngine;

public class KabaddiOpponent : MonoBehaviour
{
    [SerializeField] KabaddiManager gameManager;
    [SerializeField] GameObject target;
    Vector2 targetPosition;
    [SerializeField] float speed;
    float movementSpeed;
    Quaternion rotation;
    [SerializeField] Animator animator;

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameStart == false || gameManager.gameEnd == true)
        {
            movementSpeed = 0;
        }
        else
        {
            movementSpeed = speed * 100 * Time.deltaTime;
        }

        targetPosition = target.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed);

        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Running();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            gameManager.Lose();
        }
    }

    void Running()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running") == false && gameManager.gameStart == true)
        {
            animator.Play("Running");
        }
    }
}
