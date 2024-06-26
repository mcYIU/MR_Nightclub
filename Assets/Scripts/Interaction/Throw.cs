using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public InteractionManager interactionManager;
    private bool isGrabbed = false;

    HandGrabInteractable grab;

    private void Start()
    {
        grab = GetComponent<HandGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGrabbed)
        {
            interactionManager.ChangeLevelIndex(gameObject.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Environment"))
            if (grab.Interactors != null)
            {
                isGrabbed = true;
            }

    }
}
