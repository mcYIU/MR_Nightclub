using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Rose_Pluck : MonoBehaviour
{
    public Transform detechPoint;
    public float detachDistance;
    public AudioSource audioSource;

    private Rose rose;

    private void Start()
    {
        rose = GetComponentInParent<Rose>();
    }

    public void Pick()
    {
        if (Vector3.Distance(transform.position, detechPoint.position) < detachDistance)
        {
            audioSource.Play();
            transform.parent = null;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            rose.AddIndex();
        }
    }
}
