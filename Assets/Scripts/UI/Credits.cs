using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour

{
public void RestartGame()
    {
        
        
        if (SportsBlitzGameManager.Instance != null)
        {
            SportsBlitzGameManager.Instance.ResetStats();
        }

        // Stop all audio 
        var sounds = FindObjectsOfType<AudioSource>();
        foreach (var s in sounds)
            s.Stop();
        
        SceneManager.LoadScene(0);
    }
}