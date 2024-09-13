using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] VO_Text;
    public AudioClip[] VO_Audio;

    public Image dialogueNoticeUI;
    public Canvas dialogueCanvas;
    public Transform player;
    public InteractionManager interactionManager;

    [HideInInspector] public bool isPlayerOut = true;

    GameManager gameManager;
    DialogueManager dialogueManager;
    LightingManager lightingManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        lightingManager = FindAnyObjectByType<LightingManager>();

        dialogueManager = FindAnyObjectByType<DialogueManager>();
        dialogueCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameManager.isStarted)
            if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
            {
                //if (isPlayerOut)
                //{
                //    isPlayerOut = false;

                StartDialogue(interactionManager.LevelIndex);
                //}
            }
            else
            {
                interactionManager.PlayAudio();
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameManager.isStarted)
        {
            interactionManager.CleanNotice();

            if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
            {
                //if (!isPlayerOut)
                //{
                    EndDialogue();

                //    isPlayerOut = true;
                //}
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
        if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
        {
            dialogueNoticeUI.enabled = true;
            dialogueManager.EndDialogue();
        }
        else
        {
            dialogueNoticeUI.enabled = false;
            dialogueManager.EndDialogue();
            //lightingManager.LightSwitch_Exit(gameObject.name);
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


