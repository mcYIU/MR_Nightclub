using UnityEngine;

public class Ashtray_Put : MonoBehaviour
{
    [SerializeField] private AudioClip SFX;
    [SerializeField] Interactable interactable;
    //[SerializeField] Interactable[] interactables;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Fracture") && 
            interactable.isInteractionEnabled)
        {
            SoundEffectManager.PlaySFXOnce(SFX);

            interactable.IncreaseInteractionLevel();
            //foreach (var interactable in interactables)
            //{
            //    interactable.IncreaseInteractionLevel();
            //}
        }
    }
}
