using UnityEngine;
using UnityEngine.UI;

public class CustomerDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Canvas dialogueCanvas;
    public Image dialogueImage;
    public AudioClip dialogueAudio;

    private void Start()
    {
        dialogueImage.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            {
                dialogueImage.enabled = false;
                DialogueManager.StartDialogue(dialogue, dialogueCanvas, null);
            }
    }
}
