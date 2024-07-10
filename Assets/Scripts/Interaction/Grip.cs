using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grip : MonoBehaviour
{
    public Animator animator;
    HandGrabInteractable interactable;

    private void Start()
    {
        interactable = GetComponent<HandGrabInteractable>();
    }

    void Update()
    {
        if(interactable.Interactors.Count > 1)
        {
            animator.SetBool("IsGripped", true);
        }
    }
}
