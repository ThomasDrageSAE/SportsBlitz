using UnityEngine;

public class PlayerCarControllerTopDown : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float minX = -3f;
    public float maxX = 3f;

    private bool isAlive = true;

    private MinigameManager manager;
    private RacingTrafficLight lightSystem;
    private MiniGameTutorial tutorial;

    void Start()
    {
        manager = FindObjectOfType<MinigameManager>();
        lightSystem = FindObjectOfType<RacingTrafficLight>();
        tutorial = FindObjectOfType<MiniGameTutorial>();
    }

    void Update()
    {
        // Stop movement on win OR lose
        if (!isAlive || RacingGameManager.RacingGameOver)
            return;

        // Block movement until tutorial + GO
        if (tutorial != null && !tutorial.TutorialFinished) return;
        if (lightSystem != null && !lightSystem.sequenceFinished) return;

        float move = Input.GetAxisRaw("Horizontal");
        Vector3 pos = transform.position;
        pos.x += move * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            isAlive = false;
            RacingGameManager.RacingGameOver = true;
            manager.Lose();
        }
    }
}