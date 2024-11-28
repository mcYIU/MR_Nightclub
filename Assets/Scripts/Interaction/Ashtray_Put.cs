using UnityEngine;

public class Ashtray_Put : MonoBehaviour
{
    [SerializeField] private AudioClip SFXClip;
    [SerializeField] Interactable[] interactables;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Fracture"))
        {
            SoundEffectManager.PlaySFXOnce(SFXClip);
            
            foreach (var interactable in interactables)
            {
                interactable.IncreaseInteractionLevel();
            }
        }
    }
}
