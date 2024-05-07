using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    //public float fractureForceThreshold;

    public HandGrabInteractor leftGrabber;
    public HandGrabInteractor rightGrabber;
    public GameObject fracturedObjectPrefab;

    public float rotationRateToFracture;

    private bool isGrabbed = false;

    Quaternion leftHandRot;
    Quaternion rightHandRot;

    private void Update()
    {
        if (leftGrabber.IsGrabbing == this.gameObject 
            && rightGrabber.IsGrabbing == this.gameObject)
        {
            isGrabbed = true;
            //print(isGrabbed);

            float leftRotation = Quaternion.Angle(leftHandRot, leftGrabber.transform.rotation);
            float rightRotation = Quaternion.Angle(rightHandRot, rightGrabber.transform.rotation);
            print(leftRotation + "left");
            print(rightRotation + "right");

            if(leftRotation > rotationRateToFracture || rightRotation > rotationRateToFracture)
            {
                FractureObject();
            }
        }
        else
        {
            isGrabbed = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGrabbed)
        {
            leftHandRot = leftGrabber.transform.rotation;
            rightHandRot = rightGrabber.transform.rotation;
        }
    }

    private void FractureObject()
    {
        if (isGrabbed)
        {
            GameObject fracturedMatch = Instantiate(fracturedObjectPrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}