using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;

public class Champagne_Splash : MonoBehaviour
{
    public GameObject attachPoint;
    public ParticleSystem pouringVFX;
    public float pushForce;
    public float pouringThreadhold = 6f;
    public AudioSource openCapSound;
    public InteractionManager interactionManager;

    private Rigidbody rb;
    private HandGrabInteractable handGrab;
    private bool isOpened = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handGrab = GetComponent<HandGrabInteractable>();
    }

    void FixedUpdate()
    {
        if(isOpened)
        {
            transform.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;
            handGrab.enabled = false;

            rb.AddForce(gameObject.transform.up * pushForce, ForceMode.Impulse);
        }
    }

    public void Pouring()
    {
        isOpened = true;

        openCapSound.Play();
        pouringVFX.Play();
        interactionManager.ChangeLevelIndex(gameObject.name);
    }

}
