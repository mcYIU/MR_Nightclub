using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Match_Fracture : MonoBehaviour
{
    public HandGrabInteractor leftInteractor;
    public HandGrabInteractor rightInteractor;
    public GameObject fracturedObjectPrefab;

    private bool isGrabbed;

    private void Update()
    {
        if(isGrabbed)
        {
            if(leftInteractor.IsGrabbing == gameObject && rightInteractor.IsGrabbing == gameObject)
            {
                FractureObject();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Environment"))
        {
            isGrabbed = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            isGrabbed = true;
        }
    }

    private void FractureObject()
    {
        Instantiate(fracturedObjectPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}