using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionVisual;
    [SerializeField] AudioClip SFX;
    [SerializeField] private Interactable interactable;


    public void Poke()
    {
        Instantiate(explosionVisual);
        SoundEffectManager.PlaySFXOnce(SFX);

        interactable.IncreaseInteractionLevel();

        Destroy(gameObject);
    }
}
