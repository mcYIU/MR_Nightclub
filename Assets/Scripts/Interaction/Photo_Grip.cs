using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Photo_Grip : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip SFX;
    [SerializeField] private HandGrabInteractable[] handGrabs;
    [SerializeField] private Interactable interactable;

    public void Grip()
    {
        if (CheckHandGrabs() && interactable.isInteractionEnabled)
        {
            animator.SetBool("IsGripped", true);
            SoundEffectManager.PlaySFXOnce(SFX);

            interactable.IncreaseInteractionLevel();
        }
    }

    private bool CheckHandGrabs()
    {
        int grabCount = 0;

        for (int i = 0; i < handGrabs.Length; i++)
        {
            if (handGrabs[i].Interactors.Count > 0) grabCount++;
        }

        if (grabCount == handGrabs.Length) return true;
        else return false;
    }
}
