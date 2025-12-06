using System;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class KabaddiPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    private float movementSpeed;
    public KabaddiManager gameManager;
    [SerializeField] Animator animator;

    private void Start()
    {
        gameManager = FindObjectOfType<KabaddiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStart == false || gameManager.gameEnd == true)
        {
            movementSpeed = 0;
        }
        else
        {
            movementSpeed = speed * 100;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Quaternion rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * movementSpeed);
            Vector2 position = transform.position;
            position.y += movementSpeed * Time.deltaTime;
            transform.position = position;
            Running();
        }
        if (Input.GetKey(KeyCode.S))
        {
            Quaternion rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * movementSpeed);
            Vector2 position = transform.position;
            position.y -= movementSpeed * Time.deltaTime;
            transform.position = position;
            Running();
        }
        if (Input.GetKey(KeyCode.D))
        {
            Quaternion rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * movementSpeed);
            Vector2 position = transform.position;
            position.x += movementSpeed * Time.deltaTime;
            transform.position = position;
            Running();
        }
        if (Input.GetKey(KeyCode.A))
        {
            Quaternion rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * movementSpeed);
            Vector2 position = transform.position;
            position.x -= movementSpeed * Time.deltaTime;
            transform.position = position;
            Running();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Finish"))
        {
            gameManager.GameWin();
        }

        if(other.gameObject.CompareTag("Barrier"))
        {
            gameManager.GameLose();
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
