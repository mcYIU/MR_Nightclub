using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject dialogueNoticeUI;
    public GameObject dialogueCanvas;

    DialogueManager dialogueManager;
    LightingManager lightingManager;

    private void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        dialogueCanvas.SetActive(false);
        dialogueNoticeUI.SetActive(true);

        lightingManager = FindAnyObjectByType<LightingManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable") || !other.gameObject.CompareTag("Environment"))
        {
            lightingManager.LightSwitch_Enter(gameObject.name);

            if (!dialogueManager.isDialogueShown)
            {
                StartDialogue();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable") || !other.gameObject.CompareTag("Environment"))
        {
            lightingManager.LightSwitch_Exit(gameObject.name);

            EndDialogue();
        }
    }

    private void StartDialogue()
    {
        dialogueNoticeUI.SetActive(false);
        dialogueManager.StartDialogue(dialogue, dialogueCanvas);
    }

    private void EndDialogue()
    {
        dialogueNoticeUI.SetActive(true);
        dialogueManager.EndDialogue();
    }
}
