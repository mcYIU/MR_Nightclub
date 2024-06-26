using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;

public class Pour : MonoBehaviour
{
    public Transform bottleCap;
    public Transform attachPoint;
    public GameObject pouringVFX;
    public InteractionManager interactionManager;

    Rigidbody bottleCap_rb;
    HandGrabInteractable bottleCap_handGrab;
    private Quaternion initialRotation;
    private readonly float pourThreshold = 90f;
    private bool isOpened = false;
    private bool isPouring = false;

    void Start()
    {
        bottleCap_rb = bottleCap.GetComponent<Rigidbody>();
        bottleCap_handGrab = bottleCap.GetComponent<HandGrabInteractable>();

        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (!isOpened)
        {
            if (Vector3.Distance(bottleCap.position, attachPoint.position) > 0.001f)
            {
                isOpened = true;
            }
        }
        else
        {
            if(bottleCap.parent != null)
            {
                bottleCap_rb.isKinematic = false;
                bottleCap_handGrab.enabled = false;
                bottleCap.SetParent(null);
            }

            if(!isPouring)
            {
                float angle = Quaternion.Angle(initialRotation, transform.rotation);
                if (angle > pourThreshold)
                {
                    StartCoroutine(Pouring());
                }
            }
        }
    }

    private IEnumerator Pouring()
    {
        GameObject vfx = Instantiate(pouringVFX, attachPoint.transform.position, Quaternion.identity);
        vfx.transform.SetParent(attachPoint.transform, true);
        if (vfx.TryGetComponent<ParticleSystem>(out var vfx_particle))
        {
            vfx_particle.Play();
            isPouring = true;
        }

        yield return new WaitForSeconds(vfx_particle.main.duration);
        interactionManager.ChangeLevelIndex(gameObject.name);
    }
}
