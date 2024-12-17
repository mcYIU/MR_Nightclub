 using UnityEngine;
using UnityEngine.UI;

public class CustomerDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Components")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Image noticeUI;

    private void Start()
    {
        noticeUI.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !DialogueManager.isTalking)
            {
                noticeUI.enabled = false;
                DialogueManager.StartDialogue(dialogue, dialogueCanvas);
            }
    }
}
