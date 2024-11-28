using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] AudioClip SFX;
    [SerializeField] private Interactable interactable;


    public void Poke()
    {
        explosion.Play();
        SoundEffectManager.PlaySFXOnce(SFX);
        interactable.IncreaseInteractionLevel();

        Destroy(gameObject);
    }
}
