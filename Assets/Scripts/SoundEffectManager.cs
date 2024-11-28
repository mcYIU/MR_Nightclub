using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;
    public static AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    public static void PlaySFXOnce(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public static void PlaySFXLoop(AudioClip clip)
    {
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public static void StopSFXLoop()
    {
        audioSource.Stop();
        audioSource.loop = false;
    }
}
