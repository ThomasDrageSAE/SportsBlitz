using UnityEngine;
using UnityEngine.UI;

public class WrestlingManager : MonoBehaviour
{
    [SerializeField] private float time;
    public bool gameStart;
    public bool gameEnd;
    public bool playerJump;
    [SerializeField] private Text countdownTimer;
    [SerializeField] private float waitTime;
    private int countdownText;
    [SerializeField] private GameObject controlScreen;

    [SerializeField] private WrestlingPlayer player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waitTime = 2;
        time = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0)
        {
            controlScreen.SetActive(true);
            waitTime -= Time.deltaTime;
        }

        if (waitTime <= 0)
        {
            controlScreen.SetActive(false);
            gameStart = true;
        }

        if (gameStart && time > 0 && !gameEnd)
        {
            time -= Time.deltaTime;
        }

        if (time <= 0 && !playerJump)
        {
            player.PlayerJump();
            playerJump = true;
        }

        countdownText = (int)time;

        countdownTimer.text = countdownText.ToString();
    }
}
