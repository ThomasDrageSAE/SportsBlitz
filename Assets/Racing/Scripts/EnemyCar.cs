using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public float speed = 4f;

    void Update()
    {
        if (RacingGameManager.RacingGameOver)
            return;

        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -6f)
            Destroy(gameObject);
    }
}