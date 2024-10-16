using UnityEngine;

public class EndDialogueTrigger : MonoBehaviour
{
    public Dialogue[] VO_Text;
    public AudioClip[] VO_Audio;
    public GameObject[] characters;
    public AudioClip changeSceneAudio;

    public static int dialogueIndex = 0;

    DialogueManager dialogueManager;
    GameManager gameManager;
    private float startTime = 3.5f;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameManager = FindObjectOfType<GameManager>();

        if (dialogueManager != null) Invoke("StartDialogue", startTime);
    }

    public void ChangeToNextScene()
    {
        if (gameManager != null)
        {
            gameManager.endSceneMusic.Stop();
            gameManager.ChangeToNextScene();
            if (dialogueManager.VO != null) dialogueManager.VO.PlayOneShot(changeSceneAudio);
        }
    }

    private void StartDialogue()
    {
        dialogueManager.StartFinalDialogue(this);
    }
}
