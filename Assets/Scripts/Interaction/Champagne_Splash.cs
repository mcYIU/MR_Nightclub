using UnityEngine;

public class Champagne_Splash : MonoBehaviour
{
    public bool isBottleOpened = false;

    [SerializeField] private ParticleSystem pouringVisual;
    [SerializeField] private float pushForce;
    [SerializeField] private AudioClip SFX; 
    [SerializeField] private Interactable interactable;
    private Rigidbody rb;

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
        pouringVisual.Play();
        SoundEffectManager.PlaySFXOnce(SFX);
        interactable.IncreaseInteractionLevel();
    }

}
