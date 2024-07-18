using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTest : MonoBehaviour
{
    public SkinnedMeshRenderer face;
    OVRCustomFace ovrFace;
    float index01;
    float index02;

    void Update()
    {
        index01 = face.GetBlendShapeWeight(48);
        index02 = face.GetBlendShapeWeight(26);
        if (index01 > 90)
        {
            Debug.Log("Can");
        }
        if(index02 > 60)
        {
            Debug.Log("Do");
        }
    
    }
}
