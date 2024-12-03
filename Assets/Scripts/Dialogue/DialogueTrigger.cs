using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueTrigger : MonoBehaviour
{
    [Serializable]
    private struct DialogueUIElements
    {
        public Image noticeUI;
        public Canvas dialogueCanvas;
    }

    [Header("Dialogue")]
    public Dialogue[] dialogues;
    [SerializeField] private AudioClip completedLevelAudio;
    [Header("Dialogue UI")]
    [SerializeField] private DialogueUIElements uiElements;
    [Header("Interaction Manager")]
    [SerializeField] private InteractionManager interactionManager;

    private void Start()
    {
        InitializeDialogue();
    }

    private void InitializeDialogue()
    {
        uiElements.dialogueCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsValidTrigger(other)) return;

        HandleTriggerEnter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsValidTrigger(other)) return;

        HandleTriggerExit();
    }

    private bool IsValidTrigger(Collider other)
    {
        return other.CompareTag("Player") &&
               GameManager.IsStarted &&
               !GameManager.IsCompleted;
    }

    private void HandleTriggerEnter()
    {
        if (IsWithinInteractionLimit())
        {
            StartDialogue(interactionManager.LevelIndex);
        }
        else
        {
            HandleCompletedInteraction();
        }

    }

    private void HandleTriggerExit()
    {
        if (IsWithinInteractionLimit())
        {
            EndDialogue();
            interactionManager.CleanInteraction();
        }
    }

    private bool IsWithinInteractionLimit()
    {
        return interactionManager.LevelIndex < interactionManager.interactionLayers.Length;
    }

    private void HandleCompletedInteraction()
    {
        DialogueManager.OverrideSetAudio(completedLevelAudio, true);
    }

    public void StartDialogue(int index)
    {
        if (!IsWithinInteractionLimit()) HandleCompletedInteraction();

        if (!IsValidDialogueIndex(index)) return;

        SetupDialogueUI(false);
        InitiateDialogue(index);
    }

    private bool IsValidDialogueIndex(int index)
    {
        return index >= 0 && index < dialogues.Length;
    }

    private void SetupDialogueUI(bool isNoticeVisible)
    {
        if (IsWithinInteractionLimit())
        {
            uiElements.noticeUI.enabled = isNoticeVisible;
        }
        else
        {
            uiElements.noticeUI.enabled = false;
        }
    }

    private void InitiateDialogue(int index)
    {
        DialogueManager.StartDialogue
            (
            dialogues[index],
            uiElements.dialogueCanvas,
            interactionManager
            );
    }

    public void EndDialogue()
    {
        SetupDialogueUI(true);

        DialogueManager.EndDialogue();
    }
}