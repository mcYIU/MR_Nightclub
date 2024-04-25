using Oculus.Interaction.Samples;
using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixToCamera : MonoBehaviour
{
    public Camera vrcamera;
    public float Yoffset;
    public float Zoffset;

    private Vector3 camPos;
    private Vector3 camRotation;

    void Start()
    {

    }

    void LateUpdate()
    {
        camPos = vrcamera.transform.position;
        transform.position = new Vector3
            (camPos.x, camPos.y + Yoffset, camPos.z + Zoffset);

       //transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }
}
