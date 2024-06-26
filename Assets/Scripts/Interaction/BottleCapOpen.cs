using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;

public class BottleCapOpen : MonoBehaviour
{
    public GameObject attachPoint;
    public GameObject pouringVFX;
    public float pushForce;
    public float pouringTime;
    public InteractionManager interactionManager;

    private Rigidbody rb;
    private HandGrabInteractable handGrab;
    private bool isOpened = false;
    private bool isPouring = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handGrab = GetComponent<HandGrabInteractable>();
    }

    private void Update()
    {
        if (!isOpened && Vector3.Distance(transform.position, attachPoint.transform.position) > 0.001f)
        {
            if (!isPouring)
            {
                isOpened = true;
            }
            else
            {
                isOpened = false;
            }
        }
    }

    void FixedUpdate()
    {
        if(isOpened)
        {
            rb.isKinematic = false;
            handGrab.enabled = false;

            rb.AddForce(attachPoint.transform.up * pushForce, ForceMode.Impulse);
            rb.AddForce(Physics.gravity, ForceMode.Acceleration);

            transform.SetParent(null);

            if (!isPouring)
                StartCoroutine(Pouring());
        }
    }

    IEnumerator Pouring()
    {
        isPouring = true;

        GameObject vfx = Instantiate(pouringVFX, attachPoint.transform.position, Quaternion.identity);
        vfx.transform.SetParent(attachPoint.transform, true);

        yield return new WaitForSeconds(pouringTime);

        vfx.SetActive(false);

        interactionManager.ChangeLevelIndex(gameObject.name);
    }

}
