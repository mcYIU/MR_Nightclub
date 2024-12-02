using UnityEngine;

public class Champagne_Splash : MonoBehaviour
{
    [SerializeField] private ParticleSystem pouringVisual;
    [SerializeField] private float pushForce;
    [SerializeField] private AudioClip SFX; 
    [SerializeField] private Interactable interactable;
    private Rigidbody rb;
    private bool isBottleOpened = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(isBottleOpened && transform.parent != null)
        {
            transform.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.AddForce(gameObject.transform.up * pushForce, ForceMode.Impulse);
        }
    }

    public void Pouring()
    {
        isBottleOpened = true;

        pouringVisual.Play();
        SoundEffectManager.PlaySFXOnce(SFX);
        interactable.IncreaseInteractionLevel();
    }

}
