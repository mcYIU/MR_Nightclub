using UnityEngine;

public class FaceTo : MonoBehaviour
{
    OVRCameraRig CameraRig;
    public Transform playerPos;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            //Vector3 cameraPos = GetCameraPos();
            //transform.LookAt(cameraPos);
            Vector3 directionToPlayer = playerPos.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            Quaternion reversedRotation = targetRotation * Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = reversedRotation;
            //transform.LookAt(playerPos);
        }
    }

    Vector3 GetCameraPos()
    {
        CameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        return CameraRig.transform.position;
    }
}
