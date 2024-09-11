using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Photo_Grip : MonoBehaviour
{
    public Animator animator;
    public InteractionManager manager;
    public HandGrabInteractable[] interactables;

    public void Grip()
    {
        int grabCount = 0;

        for (int i = 0; i < interactables.Length; i++)
            if (interactables[i].Interactors.Count > 0)
                grabCount++;

        if(grabCount == interactables.Length)
        {
            animator.SetBool("IsGripped", true);
            manager.ChangeLevelIndex(gameObject.name);
        }
    }
}
