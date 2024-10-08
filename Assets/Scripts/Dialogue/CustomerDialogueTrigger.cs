using UnityEngine;

public class CustomerDialogueTrigger : MonoBehaviour
{
    public Canvas dialogueCanvas;

    private void Start()
    {
        dialogueCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dialogueCanvas != null)
            dialogueCanvas.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        dialogueCanvas.enabled = false;
    }
}
