using UnityEngine;
using UnityEngine.UI;

public class CustomerDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Canvas dialogueCanvas;
    public Image dialogueImage;
    public AudioClip dialogueAudio;

    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueImage.enabled = true;

        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && dialogueManager != null)
            if (!dialogueManager.VO.isPlaying)
            {
                dialogueImage.enabled = false;
                dialogueManager.StartDialogue(dialogue, dialogueCanvas, dialogueAudio, null);
            }
    }
}
