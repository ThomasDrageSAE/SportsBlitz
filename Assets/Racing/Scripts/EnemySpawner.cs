using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyCarPrefab;

    [Header("Lane Settings")]
    public float[] lanePositions = new float[] { -2f, 0f, 2f };

    [Header("Spawn Settings")]
    public float spawnInterval = 0.8f;
    public float spawnY = 6f;

    [Header("Vertical Spacing")]
    public float minVerticalGap = 4f;

    private float timer = 0f;
    private GameObject[] lastCarInLane;

    private MiniGameTutorial tutorial;
    private RacingTrafficLight lightSystem;

    void Start()
    {
        tutorial = FindObjectOfType<MiniGameTutorial>();
        lightSystem = FindObjectOfType<RacingTrafficLight>();

        lastCarInLane = new GameObject[lanePositions.Length];
    }

    void Update()
    {
        if (RacingGameManager.RacingGameOver) return;
        if (tutorial != null && !tutorial.TutorialFinished) return;
        if (lightSystem != null && !lightSystem.sequenceFinished) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            int laneIndex = Random.Range(0, lanePositions.Length);
            GameObject lastCar = lastCarInLane[laneIndex];

            if (lastCar != null)
            {
                float dy = spawnY - lastCar.transform.position.y;
                if (dy < minVerticalGap)
                    return;
            }

            Vector3 pos = new Vector3(lanePositions[laneIndex], spawnY, 0f);
            GameObject newCar = Instantiate(enemyCarPrefab, pos, Quaternion.identity);

            lastCarInLane[laneIndex] = newCar;
        }
    }
}