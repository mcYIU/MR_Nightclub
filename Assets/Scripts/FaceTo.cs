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
            transform.LookAt(playerPos);
        }
    }

    Vector3 GetCameraPos()
    {
        CameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        return CameraRig.transform.position;
    }
}
