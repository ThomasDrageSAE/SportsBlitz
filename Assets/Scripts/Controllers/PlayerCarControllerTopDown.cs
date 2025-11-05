using UnityEngine;

public class PlayerCarControllerTopDown : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float xLimit = 3f;

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        Vector3 pos = transform.position;
        pos.x += move * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
            FindObjectOfType<MinigameManager>().Lose();
    }
}