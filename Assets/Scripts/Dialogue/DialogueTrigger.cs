using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] VO_Text;
    public AudioClip[] VO_Audio;

    public GameObject dialogueNoticeUI;
    public GameObject dialogueCanvas;
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
        dialogueCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if 
        (other.gameObject.CompareTag("Player") && 
        interactionManager.LevelIndex < interactionManager.ineteractionLayerCount &&
        gameManager.isStarted)
            if (isPlayerOut)
            {
                isPlayerOut = false;

                StartDialogue(interactionManager.LevelIndex);
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if 
        (other.gameObject.CompareTag("Player") && 
        interactionManager.LevelIndex < interactionManager.ineteractionLayerCount &&
        gameManager.isStarted)
            if (!isPlayerOut)
            {
                EndDialogue();
                interactionManager.CleanNotice();

                isPlayerOut = true;
            }
    }

    public void StartDialogue(int index)
    {
        dialogueNoticeUI.SetActive(false);
        dialogueManager.StartDialogue(VO_Text[index], dialogueCanvas, VO_Audio[index], interactionManager);
    }

    public void EndDialogue()
    {
        if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
        {
            dialogueNoticeUI.SetActive(true);
            dialogueManager.EndDialogue();
        }
        else
        {
            dialogueNoticeUI.SetActive(false);
            dialogueManager.EndDialogue();
            lightingManager.LightSwitch_Exit(gameObject.name);
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


