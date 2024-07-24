using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;

public class Champagne_Splash : MonoBehaviour
{
    public GameObject attachPoint;
    public ParticleSystem pouringVFX;
    public float pushForce;
    public float pouringThreadhold = 6f;
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
            transform.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;
            handGrab.enabled = false;

            rb.AddForce(gameObject.transform.up * pushForce, ForceMode.Impulse);
            //rb.AddForce(Physics.gravity, ForceMode.Acceleration);

            if (!isPouring)
                StartCoroutine(Pouring());
        }
    }

    IEnumerator Pouring()
    {
        isPouring = true;

        //GameObject vfx = Instantiate(pouringVFX, attachPoint.transform.position, Quaternion.identity);
        //vfx.transform.SetParent(attachPoint.transform, true);

        //float pouringTime = vfx.GetComponent<ParticleSystem>().time;
        
        pouringVFX.Play();

        yield return new WaitForSeconds(pouringThreadhold);
        pouringVFX.Stop();

        interactionManager.ChangeLevelIndex(gameObject.name);
    }

}
