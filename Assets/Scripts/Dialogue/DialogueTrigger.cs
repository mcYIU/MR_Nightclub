using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] VO_Text;
    public AudioClip[] VO_Audio;

    public GameObject dialogueNoticeUI;
    public GameObject dialogueCanvas;
    public Transform player;
    //public float triggerDistance;
    public InteractionManager interactionManager;

    [HideInInspector] public bool canTalk = false;
    [HideInInspector] public bool isPlayerOut = true;

    DialogueManager dialogueManager;
    LightingManager lightingManager;

    private void Start()
    {
        lightingManager = FindAnyObjectByType<LightingManager>();

        dialogueManager = FindAnyObjectByType<DialogueManager>();
        dialogueCanvas.SetActive(false);
        //dialogueNoticeUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if 
        (other.gameObject.CompareTag("Player") && 
        interactionManager.LevelIndex < interactionManager.ineteractionLayerCount && 
        canTalk)
            if (isPlayerOut)
            {
                isPlayerOut = false;

                //lightingManager.LightSwitch_Enter(gameObject.name);
                StartDialogue(interactionManager.LevelIndex);
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if 
        (other.gameObject.CompareTag("Player") && 
        interactionManager.LevelIndex < interactionManager.ineteractionLayerCount && 
        canTalk)
            if (!isPlayerOut)
            {
                EndDialogue();

                //lightingManager.LightSwitch_Exit(gameObject.name);
                interactionManager.CleanNotice();

                isPlayerOut = true;
            }
    }

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

    public void StartDialogue(int index)
    {
        dialogueNoticeUI.SetActive(false);
        dialogueManager.StartDialogue(VO_Text[index], dialogueCanvas, VO_Audio[index], interactionManager);
    }

    public void StartFinalDialogue(int index)
    {
        dialogueManager.StartFinalText(VO_Text[index], VO_Audio[index], interactionManager);
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


