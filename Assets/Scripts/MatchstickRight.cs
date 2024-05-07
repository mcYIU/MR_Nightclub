using UnityEngine;

public class MatchstickRight : MonoBehaviour
{
    public bool isGrabbed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GameController"))
        {
            Debug.Log(gameObject.tag);
            // Find the parent object (Matchstick prefab) and make it a child of the controller's transform
            Transform matchstickParent = transform.parent;
            matchstickParent.SetParent(collision.transform);

            // Adjust the position and rotation of the matchstick relative to the controller
            matchstickParent.localPosition = Vector3.zero;
            matchstickParent.localRotation = Quaternion.identity;

            isGrabbed = true;
        }
    }
}