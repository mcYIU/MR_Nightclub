using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [HideInInspector] public bool isPlayerOut = true;

    public Dialogue[] VO_Text;
    public AudioClip[] VO_Audio;
    public string transitionText;

    public Image dialogueNoticeUI;
    public Canvas dialogueCanvas;
    public Transform player;
    public InteractionManager interactionManager;

    DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        dialogueCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.isStarted && !GameManager.isCompleted)
            if (interactionManager.LevelIndex < InteractionManager.ineteractionLayerCount)
            {
                if (isPlayerOut)
                {
                    isPlayerOut = false;

                    StartDialogue(interactionManager.LevelIndex);
                }
            }
            else
            {
                interactionManager.PlayAudio();
                //interactionManager.DisplayNotice(transitionText);
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.isStarted && !GameManager.isCompleted)
        {
            interactionManager.CleanNotice();

            if (interactionManager.LevelIndex < InteractionManager.ineteractionLayerCount)
            {
                if (!isPlayerOut)
                {
                    EndDialogue();

                    isPlayerOut = true;
                }
            }
        }
    }

    public void StartDialogue(int index)
    {
        dialogueNoticeUI.enabled = false;
        dialogueManager.StartDialogue(VO_Text[index], dialogueCanvas, VO_Audio[index], interactionManager);
    }

    public void EndDialogue()
    {
        if (interactionManager.LevelIndex < InteractionManager.ineteractionLayerCount)
        {
            dialogueNoticeUI.enabled = true;
            dialogueManager.EndDialogue();
        }
        else
        {
            dialogueNoticeUI.enabled = false;
            dialogueManager.EndDialogue();
        }
    }
}

//BackUp//
/*private void Update()
{
 if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
 {
     float distance = Vector3.Distance(player.position, gameObject.transform.position);
     if (distance < triggerDistance)
     {
         if (!isPlayerStaying)
         {
             isPlayerStaying = true;

             lightingManager.LightSwitch_Enter(gameObject.name);
             StartDialogue(interactionManager.LevelIndex);
         }
     }
     else
     {
         if (isPlayerStaying)
         {
             EndDialogue();

             lightingManager.LightSwitch_Exit(gameObject.name);
             interactionManager.CleanNotice();

             isPlayerStaying = false;
         }
     }
 }         
}*/


