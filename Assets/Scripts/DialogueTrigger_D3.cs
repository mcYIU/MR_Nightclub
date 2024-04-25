using UnityEngine;

public class DialogueTrigger_D3 : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager_D3 dialogueManager_d3;

    private void OnTriggerEnter(Collider other)
    {
        dialogueManager_d3.StartDialogue(dialogue);
    }
}
