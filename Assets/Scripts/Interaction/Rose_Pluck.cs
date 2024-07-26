using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Rose_Pluck : MonoBehaviour
{
    public Transform detechPoint;
    public float detachDistance;
    private Rose rose;

    private void Start()
    {
        rose = GetComponentInParent<Rose>();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, detechPoint.position) > detachDistance)
        {
            transform.parent = null;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            rose.AddIndex();
        }
    }
}
