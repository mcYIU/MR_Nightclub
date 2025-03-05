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
    public DialogueData[] data;
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

        ResetDialogueData();
    }

    private void OnTriggerStay(Collider other)
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
        }
    }

    private bool IsWithinInteractionLimit()
    {
        if (interactionManager == null) return false;

        return interactionManager.LevelIndex < interactionManager.interactionLayers.Length;
    }

    public void StartDialogue(int index = 0)
    {
        if (!IsValidDialogueIndex(index)) return;

        InitiateDialogue(index);
        SetupDialogueUI(false);
    }

    public void EnableInteraction()
    {
        if (interactionManager != null && IsWithinInteractionLimit())
        {
            interactionManager.EnableInteraction();
        }
        else
        {
            DialogueManager.isTalking = false;
        }
    }

    private void ResetDialogueData()
    {
       foreach(var _data in data)
        {
            _data.Reset();
        }
    }

    private bool IsValidDialogueIndex(int index)
    {
        return index >= 0 && index < data.Length;
    }

    private void SetupDialogueUI(bool isNoticeVisible)
    {
        if (UIElements.noticeUI != null)
        {
            UIElements.noticeUI.enabled = isNoticeVisible;
        }
    }

    private void InitiateDialogue(int index)
    {
        DialogueManager.StartDialogue(data[index], UIElements.dialogueCanvas, this);
    }

    public void EndDialogue()
    {
        SetupDialogueUI(false);

        DialogueManager.EndDialogue();
    }
}