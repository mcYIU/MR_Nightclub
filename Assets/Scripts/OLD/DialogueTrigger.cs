using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            Dialogue();
    }

    public void Dialogue()
    {
        dialogueManager.StartDialogue(dialogue);
    }
}
