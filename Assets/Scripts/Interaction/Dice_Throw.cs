using UnityEngine;

public class Dice_Throw : MonoBehaviour
{
    public static bool isGrabbed = false;
    [SerializeField] private Interactable interactable;
    [SerializeField] private AudioClip[] SFX;
    [SerializeField] private float throwForce;

    //private Rigidbody rb;

    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Environment") && isGrabbed)
        {
            ThrowOnTable();
        }
    }

    private void ThrowOnTable()
    {
        int soundIndex = Random.Range(0, SFX.Length);
        SoundEffectManager.PlaySFXOnce(SFX[soundIndex]);

        interactable.IncreaseInteractionLevel();
    }

    public void GrabAllDice(Transform[] otherTransforms, Rigidbody[] allRigidbodies)
    {
        //rb.useGravity = false;
        //rb.isKinematic = true;
        //rb.interpolation = RigidbodyInterpolation.None;

        //transform.position = parentDice.position;
        //transform.rotation = parentDice.rotation;
        //transform.SetParent(parentDice.transform);
        //transform.localScale = Vector3.one;

        //isGrabbed = true;

        foreach (Rigidbody rb in allRigidbodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;
        }

        foreach (Transform t in otherTransforms)
        {          
            t.position = transform.position;
            t.rotation = transform.rotation;
            t.SetParent(transform);
            t.localScale = Vector3.one;
        }
    }

    public void ReleaseAllDice(Transform[] otherTransforms, Rigidbody[] allRigidbodies)
    {
        //transform.SetParent(null);
        //transform.localScale = Vector3.one;

        //rb.isKinematic = false;
        //rb.useGravity = true;
        //rb.interpolation = RigidbodyInterpolation.Interpolate;
        //rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        foreach (Transform t in otherTransforms)
        {
            t.SetParent(null);
            t.localScale = Vector3.one;
        }

        foreach (Rigidbody rb in allRigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.AddForce(rb.transform.forward * throwForce, ForceMode.Impulse);
        }    
    }
}
