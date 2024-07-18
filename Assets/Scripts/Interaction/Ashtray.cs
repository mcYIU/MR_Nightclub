using UnityEngine;

public class Ashtray : MonoBehaviour
{
    public GameObject interactionObject;
    InteractionManager interactionManager;

    private void Start()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        interactionManager.ChangeLevelIndex(interactionObject.name);
    }
}
