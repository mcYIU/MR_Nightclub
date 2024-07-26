using UnityEngine;

public class Ashtray : MonoBehaviour
{
    public GameObject interactionObject;
    public InteractionManager interactionManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Fracture"))
        {
            interactionManager.ChangeLevelIndex(interactionObject.name);
        }
    }
}
