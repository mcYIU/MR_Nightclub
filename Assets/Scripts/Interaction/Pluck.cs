using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Pluck : MonoBehaviour
{
    public float detachDistance;
    Rose rose;
    Transform parentTransform;

    private void Start()
    {
        parentTransform = transform.parent.transform;
        rose = GetComponentInParent<Rose>();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, parentTransform.position) > detachDistance)
        {
            transform.parent = null;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            rose.AddIndex();
        }
    }
}
