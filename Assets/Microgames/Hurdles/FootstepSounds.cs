using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip[] sounds;
    private int currentSoundIndex = 0;
    private int lastSound;

    public void Footstep()
    {
        if (sounds.Length > 0)
        {
            audioSource.PlayOneShot(sounds[currentSoundIndex]);

            currentSoundIndex = Random.Range(0, sounds.Length);

            if (lastSound == currentSoundIndex)
            {
                currentSoundIndex++;
            }
            else
            {
                lastSound = currentSoundIndex;
            }

            if (currentSoundIndex >= sounds.Length)
            {
                currentSoundIndex = 0;
            }
        }
        
    }

}
