using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionVisual;
    [SerializeField] AudioClip SFX;
    [SerializeField] private Interactable interactable;


    public void Poke()
    {
        if (interactable.isInteractionEnabled) 
        {
            Instantiate(explosionVisual, transform.position, transform.rotation);
            SoundEffectManager.PlaySFXOnce(SFX);

            interactable.IncreaseInteractionLevel();

            Destroy(gameObject);
        }
    }
}
