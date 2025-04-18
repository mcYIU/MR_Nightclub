using Oculus.Interaction.Body.PoseDetection;
using UnityEngine;

public class DanceQuestManager : MonoBehaviour
{
    [HideInInspector] public bool isActive = false;
    [SerializeField] private CustomerDialogueTrigger customerDialogue;
    [SerializeField] private BodyPoseComparerActiveState[] poses;
    private int numCompletedPoses = 0;

    private void Start()
    {
        ResetQuest(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !IsQuestCompleted())
        {
            isActive = !isActive;
            NextQuest();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !IsQuestCompleted())
        {
            isActive = !isActive;
            ResetQuest(isActive);
        }
    }

    private bool IsQuestCompleted()
    {
        return numCompletedPoses == poses.Length;
    }

    private void ResetQuest(bool _isActive)
    {
        foreach (var p in poses) 
        {
            p.enabled = false;
            p.gameObject.SetActive(_isActive);
        }
    }

    private void NextQuest()
    {
        ResetQuest(isActive);

        poses[numCompletedPoses].enabled = true;
    }

    public void CheckPoseCompletion()
    {
        numCompletedPoses++;

        if (IsQuestCompleted())
        {
            if (customerDialogue) customerDialogue.Talk();

            isActive = !isActive;
            ResetQuest(isActive);
        }
        else
            NextQuest();
    }
}
