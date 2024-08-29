using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Dice_Throw : MonoBehaviour
{
    public InteractionManager interactionManager;
    public AudioClip[] diceSound;
    public float throwForce;

    private bool isGrabbed = false;
    private HandGrabInteractable grab;
    private Rigidbody rb;
    private AudioSource audioSource;

    private void Start()
    {
        grab = GetComponent<HandGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Environment") && isGrabbed)
        {
            int soundIndex = Random.Range(0, diceSound.Length);
            audioSource.PlayOneShot(diceSound[soundIndex]);

            interactionManager.ChangeLevelIndex(gameObject.name);
        }
    }

    public void GrabAllDice(Transform parentDice)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        transform.position = parentDice.position;
        transform.SetParent(parentDice.transform);

        isGrabbed = true;
    }

    public void ReleaseAllDice()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }
}
