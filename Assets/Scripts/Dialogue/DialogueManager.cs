using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public AudioSource VO;
    public TextMeshProUGUI finalText;
    [SerializeField] private float dialogueInterval;
    [SerializeField] private Animator crossfade;

    private static float dialogueTime;
    private readonly float defaultSentenceTime = 6.0f;
    private static Queue<string> dialogueQueue;
    private static Canvas dialogueCanvas;
    private static TextMeshProUGUI dialogueText;

    InteractionManager interactionManager;
    EndDialogueTrigger finalDialogue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dialogueQueue = new Queue<string>();

        instance.finalText.text = null;
    }

    public static void StartDialogue(Dialogue _dialogue, Canvas _canvas, AudioClip _audio, InteractionManager _manager)
    {
        if (_manager != null)
        {
            if (_manager.noticeSystem.isNoticed)
            {
                _manager.DisplayNotice(_manager.noticeSystem.noticeText[_manager.LevelIndex]);
                return;
            }
            instance.interactionManager = _manager;
        }

        if (_dialogue == null) return;

        if (instance.VO != null) instance.VO.PlayOneShot(_audio);

        foreach (string _sentence in _dialogue.sentences)
        {
            dialogueQueue.Enqueue(_sentence);
        }
        dialogueCanvas = _canvas;
        dialogueTime = _audio.length / _dialogue.sentences.Length;
        NextSentence();
    }

    public void StartFinalDialogue(EndDialogueTrigger _endDialogue)
    {
        if (finalDialogue == null) finalDialogue = _endDialogue;

        if (!finalDialogue.characters[EndDialogueTrigger.dialogueIndex].activeSelf)
            finalDialogue.characters[EndDialogueTrigger.dialogueIndex].SetActive(true);

        //isPlayCompleted = false;
        if (finalDialogue.VO_Audio[EndDialogueTrigger.dialogueIndex] != null)
        {
            VO.clip = finalDialogue.VO_Audio[EndDialogueTrigger.dialogueIndex];
            VO.Play();
        }

        foreach (string sentence in finalDialogue.VO_Text[EndDialogueTrigger.dialogueIndex].sentences)
        {
            dialogueQueue.Enqueue(sentence);
        }
        dialogueCanvas = null;

        dialogueTime = (VO.clip != null) ?
            VO.clip.length / finalDialogue.VO_Text[EndDialogueTrigger.dialogueIndex].sentences.Length : defaultSentenceTime;

        NextSentence();
    }

    private static void NextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();

            if (instance.interactionManager != null)
                instance.interactionManager.PlayAudio();
            else if (instance.finalDialogue != null)
                instance.StartCoroutine(instance.NextFinalDialogue());
        }
        else
        {
            string sentence = dialogueQueue.Dequeue();
            instance.StopAllCoroutines();
            instance.StartCoroutine(Type(sentence));
        }
    }

    private static IEnumerator Type(string sentence)
    {
        if (dialogueCanvas != null)
        {
            dialogueText = dialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
            dialogueText.text = "";
            dialogueCanvas.enabled = true;

            int currentIndex = 0;

            while (currentIndex < sentence.Length)
            {
                char currentChar = sentence[currentIndex];
                if (currentChar == '.')
                    dialogueText.text += "\n";
                else
                    dialogueText.text += currentChar;
                currentIndex++;
            }
        }
        else
        {
            instance.finalText.text = sentence;
        }

        yield return new WaitForSeconds(dialogueTime);
        NextSentence();
    }

    private IEnumerator NextFinalDialogue()
    {
        finalText.text = "";

        crossfade.SetBool("IsEyeClosed", true);

        yield return new WaitForSeconds(dialogueInterval);

        finalDialogue.characters[EndDialogueTrigger.dialogueIndex].SetActive(false);

        EndDialogueTrigger.dialogueIndex++;

        crossfade.SetBool("IsEyeClosed", false);

        if (EndDialogueTrigger.dialogueIndex == finalDialogue.characters.Length)
            finalDialogue.StartChangeSceneDialogue();
        else
        {
            finalDialogue.characters[EndDialogueTrigger.dialogueIndex].SetActive(true);

            yield return new WaitForSeconds(dialogueInterval);
            StartFinalDialogue(finalDialogue);
        }
    }

    public static void EndDialogue()
    {
        CleanText();
        instance.VO.Stop();
    }

    private static void CleanText()
    {
        instance.StopAllCoroutines();
        dialogueQueue.Clear();
        if (dialogueCanvas != null)
        {
            dialogueCanvas.enabled = false;
            dialogueCanvas = null;
            dialogueText.text = "";
        }
        else
        {
            instance.finalText.text = null;
        }
    }
}