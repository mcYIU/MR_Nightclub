using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Match_Fracture : MonoBehaviour
{
    public GameObject fracturedObjectPrefab;
    public HandGrabInteractor leftInteractor;
    public HandGrabInteractor rightInteractor;

    private void Update()
    {
        if (leftInteractor.IsGrabbing == gameObject && rightInteractor.IsGrabbing == gameObject)
        {
            FractureObject();
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
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
    }*/

    public void FractureObject()
    {
        //Debug.Log(interactable.Interactors.Count);
        //if (interactable.Interactors.Count > 1)
        //{
            Instantiate(fracturedObjectPrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        //}
    }
}