using Obi;
using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;

public class WhiskeyPour : MonoBehaviour
{
    public GameObject attachPoint;
    public GameObject obiFluid;
    public float dissolveDuration = 4f;
    public InteractionManager interactionManager;

    private Rigidbody rb;
    private HandGrabInteractable handGrab;
    private float dot_AttachPoint;
    private bool isOpened = false;
    private bool isPouring = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handGrab = GetComponent<HandGrabInteractable>();
        obiFluid.SetActive(false);
    }

    private void Update()
    {
        if (!isOpened && Vector3.Distance(transform.position, attachPoint.transform.position) > 0.001f)
        {
            isOpened = true;
        }

        if (isOpened)
        {
            dot_AttachPoint = Vector3.Dot(transform.up, Vector3.up);
            if (dot_AttachPoint < 0)
            {
                StartCoroutine(Pouring());
            }
        }
    }

    void OpenCap()
    {
        if (isOpened)
        {
            transform.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;
            handGrab.enabled = false;
        }
    }

    IEnumerator Pouring()
    {
        obiFluid.SetActive(true);
        obiFluid.TryGetComponent<ObiEmitter>(out ObiEmitter obiEmitter);
        yield return new WaitForSeconds(10f);

        interactionManager.ChangeLevelIndex(gameObject.name);
        Drying();
    }

    void Drying()
    {
        obiFluid.TryGetComponent<ObiParticleRenderer>(out ObiParticleRenderer renderer);
        float currentAlpha = renderer.particleColor.a;
        currentAlpha -= Time.deltaTime / dissolveDuration;
        currentAlpha = Mathf.Clamp01(currentAlpha);
        renderer.particleColor.a = currentAlpha;
    }


}
