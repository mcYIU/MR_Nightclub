using UnityEngine;

public class Ashtray : MonoBehaviour
{
    public GameObject interactionObject;
    public AudioSource audioSource;
    public InteractionManager interactionManager;

    string interactablesName;

    private void Start()
    {
        interactablesName = interactionObject.name;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Fracture"))
        {
            audioSource.Play();
            interactionManager.ChangeLevelIndex(interactablesName);
        }
    }
}
