using UnityEngine;
using UnityEngine.UI;

public class CustomerDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Components")]
    [SerializeField] private DialogueData data;
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Image noticeUI;

    private void Start()
    {
        noticeUI.enabled = true;
    }

    public void Talk()
    {
        if (!DialogueManager.isTalking)
        {
            noticeUI.enabled = false;
            DialogueManager.StartDialogue(data, dialogueCanvas);
        }
    }
}
