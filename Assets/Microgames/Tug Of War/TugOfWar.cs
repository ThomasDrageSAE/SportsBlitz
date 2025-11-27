using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SportsBlitz.Controls.Managers;
using SportsBlitz.Events;

public class TugOfWar : MonoBehaviour
{
    [Header("Rope Settings")]
    public Transform win;                  // The win object
    public float autoMoveSpeed = 2f;        // Rope moves right automatically
    public float playerMoveSpeed = 0.5f;    // Rope moves left per key press (fixed distance)

    [Header("Win/Lose Settings")]
    public Transform playerLocation;             // set to player
    public Transform enemyLocation;            // set to enemy
    public float winDistance = 0.5f;        // Distance to win
    public float loseDistance = 0.5f;       // Distance to lose

    private MinigameManager minigame;

    [Header("Debug")]
    [SerializeField] private bool debug = true;

    private string selectedKey;               // The single key to mash

    private void Start()
    {
        SelectSingleKey();
    }

    private void Update()
    {
        AutoMoveRope();
        CheckPlayerPress();
        CheckWinLose();
    }

    #region Rope Movement
    void AutoMoveRope()
    {
        // Smooth automatic right movement
        win.Translate(Vector3.right * autoMoveSpeed * Time.deltaTime, Space.World);
    }
    #endregion

    #region Player Input
    void CheckPlayerPress()
    {
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager == null) return;

        if (Input.GetKeyDown(selectedKey.ToString().ToLower()))
        {
            // Move rope left by a fixed amount
            win.Translate(Vector3.left * playerMoveSpeed, Space.World);

            // Trigger UI effects (green/shrink)
            EventManager.Instance?.correctKeyInput?.Invoke(selectedKey.ToString().ToUpper());

            // Reset the key UI after a short delay (0.2 seconds)
            StartCoroutine(ResetKeyUI(selectedKey, 0.2f));

            if (debug) Debug.Log($"Correct key '{selectedKey}' pressed. Rope moved left.");
        }
    }

    private IEnumerator ResetKeyUI(string key, float delay)
    {
        yield return new WaitForSeconds(delay);

        KeyInputUI keyUI = GameObject.Find(key.ToString().ToUpper() + "_Key")?.GetComponent<KeyInputUI>();
        if (keyUI != null)
            keyUI.Pressed(false); 
    }
    #endregion

    #region Key Selection
    void SelectSingleKey()
    {
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager == null || UIManager.Instance == null) return;

        // Generate exactly one random key
        List<string> newKeys = inputManager.GenerateRandomChars(1, false, false);
        selectedKey = newKeys[0];

        // Clear any previous keys in InputManager
        inputManager.ClearNeededKeys();
        inputManager.GetNeededKeys().Add(selectedKey);

        // Clear UI and spawn this key
        UIManager.Instance.ClearUI();
        List<GameObject> prefabs = new List<GameObject>
        {
            inputManager.GetPrefabForLetter(selectedKey)
        };
        UIManager.Instance.CreateUI(1, new List<string> { selectedKey }, prefabs);

        if (debug) Debug.Log("Selected key for game: " + selectedKey);
    }
    #endregion

    #region Win/Lose Condition
    void CheckWinLose()
    {
        // Win
        if (Vector3.Distance(win.position, playerLocation.position) <= winDistance)
        {
            Debug.Log("You Win!");
            minigame.Win();
        }
        // Lose
        else if (Vector3.Distance(win.position, enemyLocation.position) <= loseDistance)
        {
            Debug.Log("You Lose!");
            minigame.Lose();
        }
    }
    #endregion
}
