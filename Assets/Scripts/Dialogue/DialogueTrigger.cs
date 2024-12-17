using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueTrigger : MonoBehaviour
{
    [Serializable]
    public struct DialogueUIElements
    {
        public Image noticeUI;
        public Canvas dialogueCanvas;
    }

    [Header("Dialogue")]
    public Dialogue[] dialogues;
    [Header("Dialogue UI")]
    public DialogueUIElements UIElements;
    [Header("Interaction Manager")]
    [SerializeField] InteractionManager interactionManager;

    [HideInInspector] private bool isDialogueTriggered = false;

    private void Start()
    {
        InitializeDialogue();
    }

    private void InitializeDialogue()
    {
        UIElements.dialogueCanvas.enabled = false;
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
               !GameManager.IsCompleted &&
               !DialogueManager.isTalking;
    }

    private void HandleTriggerEnter()
    {
        if (!isDialogueTriggered)
        {
            StartDialogue(interactionManager.LevelIndex);

            isDialogueTriggered = true;
        }
    }

    private void HandleTriggerExit()
    {
        if (!IsWithinInteractionLimit() && isDialogueTriggered)
        {
            EndDialogue();
            interactionManager.CleanInteraction();
            DialogueManager.isTalking = false;

            isDialogueTriggered = false;
            Debug.Log(isDialogueTriggered);
        }
    }

    private bool IsWithinInteractionLimit()
    {
        return interactionManager.LevelIndex < interactionManager.interactionLayers.Length;
    }

    public void StartDialogue(int index)
    {
        if (!IsValidDialogueIndex(index)) return;

        SetupDialogueUI(false);
        InitiateDialogue(index);
    }

    public void EnableInteraction()
    {
        if (interactionManager && IsWithinInteractionLimit())
        {
            interactionManager.EnableInteraction();
        }
        else
        {
            DialogueManager.isTalking = false;
        }
    }

    private bool IsValidDialogueIndex(int index)
    {
        return index >= 0 && index < dialogues.Length;
    }

    private void SetupDialogueUI(bool isNoticeVisible)
    {
        if (IsWithinInteractionLimit())
        {
            UIElements.noticeUI.enabled = isNoticeVisible;
        }
        else
        {
            UIElements.noticeUI.enabled = false;
        }
    }

    private void InitiateDialogue(int index)
    {
        DialogueManager.StartDialogue(dialogues[index], UIElements.dialogueCanvas, this);
    }

    public void EndDialogue()
    {
        SetupDialogueUI(true);

        DialogueManager.EndDialogue();
    }
}