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
    private float startTime = 2.0f;

    private void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        gameManager = FindAnyObjectByType<GameManager>();

        if (dialogueManager != null) Invoke("StartDialogue", startTime);
    }

    public void ChangeToNextScene()
    {
        if (dialogueManager.VO != null)
        {
            gameManager.endSceneMusic.Stop();
            dialogueManager.VO.PlayOneShot(changeSceneAudio);
        }

        if (gameManager != null)
            gameManager.ChangeToNextScene();
    }

    private void StartDialogue()
    {
        dialogueManager.StartFinalDialogue(this);
    }
}
