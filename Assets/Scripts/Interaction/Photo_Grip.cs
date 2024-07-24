using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Photo_Grip : MonoBehaviour
{
    public Animator animator;
    public InteractionManager manager;
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
            manager.ChangeLevelIndex(gameObject.name);
        }
    }
}
