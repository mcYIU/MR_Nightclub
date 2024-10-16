using UnityEngine;

public class CustomerDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Canvas dialogueCanvas;
    public AudioClip dialogueAudio;

    DialogueManager dialogueManager;

    private void Start()
    {
        dialogueCanvas.enabled = false;

        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && dialogueManager != null)
            if (!dialogueManager.VO.isPlaying)
                dialogueManager.StartDialogue(dialogue, dialogueCanvas, dialogueAudio, null);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && dialogueManager != null)
            dialogueManager.EndDialogue();
    }
}
