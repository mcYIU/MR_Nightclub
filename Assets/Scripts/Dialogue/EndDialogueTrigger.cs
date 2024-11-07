using UnityEngine;
using System.Collections;
using TMPro;

public class EndDialogueTrigger : MonoBehaviour
{
    [Header("Character Dialogue")]
    public Dialogue[] VO_Text;
    public AudioClip[] VO_Audio;
    public GameObject[] characters;
    public AudioClip changeSceneAudio;

    [Header("Change Scene Dialogue")]
    public Dialogue dialogue;
    public TextMeshProUGUI TMP;
    public float dialogueTime;
    public ParticleSystem particle;

    public static int dialogueIndex = 0;
    private readonly float startTime = 3.5f;
    DialogueManager dialogueManager;
    GameManager gameManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameManager = FindObjectOfType<GameManager>();

        if (TMP != null) TMP.enabled = false;

        if (dialogueManager != null) Invoke(nameof(StartDialogue), startTime);
    }

    public void StartChangeSceneDialogue()
    {
        if (particle != null) particle.Play();

        if(gameManager != null) gameManager.transitionMusic.Stop();

        if (TMP != null) TMP.enabled = true;

        //StartCoroutine(Type());
        //if (dialogueManager.VO != null) dialogueManager.VO.PlayOneShot(changeSceneAudio);
    }

    private void StartDialogue()
    {
        dialogueManager.StartFinalDialogue(this);
    }

    private IEnumerator Type()
    {
        foreach (string _sentence in dialogue.sentences)
        {
            TMP.text = _sentence;
            yield return new WaitForSeconds(dialogueTime);
        }

        gameManager.ChangeToNextScene();
    }
}
