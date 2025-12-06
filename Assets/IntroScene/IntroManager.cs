using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public string nextSceneName;
    public AudioClip clickSound;
    private AudioSource audioSource;
    private bool clicked = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (!clicked && Input.GetMouseButtonDown(0))
        {
            clicked = true;
            StartCoroutine(PlayClickAndLoad());
        }
    }

    private System.Collections.IEnumerator PlayClickAndLoad()
    {
        
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);

       
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayMusic();

       
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(nextSceneName);
    }
}