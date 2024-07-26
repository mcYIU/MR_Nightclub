using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Dice_Throw : MonoBehaviour
{
    public InteractionManager interactionManager;

    private bool isGrabbed = false;
    private HandGrabInteractable grab;
    private Rigidbody rb;

    private void Start()
    {
        grab = GetComponent<HandGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGrabbed)
            if (collision.collider.gameObject.CompareTag("Environment"))
            {
                interactionManager.ChangeLevelIndex(gameObject.name);
            }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (grab.Interactors != null)
        {
            isGrabbed = true;
        }
    }

    public void GrabAllDice(Transform parentDice)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        transform.position = parentDice.position;
        transform.SetParent(parentDice.transform);
    }

    public void ReleaseAllDice()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.AddForce(transform.forward * 0.001f, ForceMode.Impulse);
    }
}
