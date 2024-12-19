using UnityEngine;

public class Champagne_Splash : MonoBehaviour
{
    [SerializeField] private ParticleSystem pouringVisual;
    [SerializeField] private GameObject capPrefab;
    [SerializeField] private float pushForce;
    [SerializeField] private AudioClip SFX; 
    [SerializeField] private Interactable interactable;
    private MeshRenderer mesh;
    private Rigidbody cap_rb;

    private bool isBottleOpened = false;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
        if(isBottleOpened)
        {
            MoveCap(cap_rb);
        }
    }

    public void Pouring()
    {
        if (interactable.isInteractionEnabled)
        {
            pouringVisual.Play();
            SoundEffectManager.PlaySFXOnce(SFX);

            interactable.SetInteraction(false);
            interactable.IncreaseInteractionLevel();

            ReleaseCap();
        }
    }

    private void ReleaseCap()
    {
        mesh.enabled = false;

        Instantiate(capPrefab, transform.position, transform.rotation);
        cap_rb = capPrefab.GetComponent<Rigidbody>();

        isBottleOpened = true;
    }

    private void MoveCap(Rigidbody _rb)
    {
        _rb.AddForce(gameObject.transform.up * pushForce, ForceMode.Impulse);
    }
}
