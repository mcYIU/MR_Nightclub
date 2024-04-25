using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FaceTo : MonoBehaviour
{
    public Transform playerPos;

    void Start()
    {

    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.LookAt(playerPos);
        }
    }
}
