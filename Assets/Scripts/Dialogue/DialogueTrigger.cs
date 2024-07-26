using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] VO_Text;
    public AudioClip[] VO_Audio;

    public GameObject dialogueNoticeUI;
    public GameObject dialogueCanvas;
    //public TextMeshProUGUI dialogueText;
    public Transform player;
    public float triggerDistance;

    public InteractionManager interactionManager;

    private bool isPlayerStaying = false;

    DialogueManager dialogueManager;
    LightingManager lightingManager;

    private void Start()
    {
        lightingManager = FindAnyObjectByType<LightingManager>();

        dialogueManager = FindAnyObjectByType<DialogueManager>();
        dialogueCanvas.SetActive(false);
        //dialogueText.text = "";
        dialogueNoticeUI.SetActive(true);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && interactionManager.levelIndex < interactionManager.ineteractionLayerCount)
            if (!dialogueManager.isDialogueShowing || !dialogueManager.VO.isPlaying)
            {
                isPlayerStaying = true;
                lightingManager.LightSwitch_Enter(gameObject.name);
                StartDialogue(interactionManager.levelIndex);
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerStaying = false;
            if (interactionManager.levelIndex < interactionManager.ineteractionLayerCount)
            {
                lightingManager.LightSwitch_Exit(gameObject.name);
                EndDialogue();
            }
        }
    }*/

    private void Update()
    {
        float distance = Vector3.Distance(player.position, gameObject.transform.position);
        if (distance < triggerDistance && interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
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
                lightingManager.LightSwitch_Exit(gameObject.name);
                EndDialogue();
                interactionManager.CleanNotice();
                StopAllCoroutines();

                isPlayerStaying = false;               
            }
        }
    }

    public void StartDialogue(int index)
    {
        dialogueNoticeUI.SetActive(false);
        if (index < interactionManager.ineteractionLayerCount)
        {
            dialogueManager.StartDialogue(VO_Text[index], dialogueCanvas, VO_Audio[index], interactionManager);
        }
        else
        {
            dialogueManager.StartDialogue(VO_Text[index], dialogueCanvas, null, null);
        }

    }

    public void EndDialogue()
    {
        if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
        {
            dialogueNoticeUI.SetActive(true);
            dialogueManager.EndDialogue();
        }
    }
}


