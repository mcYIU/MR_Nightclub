using UnityEngine;

public class Ashtray : MonoBehaviour
{
    public GameObject interactionObject;
    public AudioSource audioSource;
    public InteractionManager interactionManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Fracture"))
        {
            audioSource.Play();
            interactionManager.ChangeLevelIndex(interactionObject.name);
        }
    }
}
