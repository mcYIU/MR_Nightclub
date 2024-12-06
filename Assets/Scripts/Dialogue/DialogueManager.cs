using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public AudioSource VO;
    public TextMeshProUGUI finalText;
    public float dialogueInterval;
    public Animator crossfade;

    //private bool isPlayCompleted = false;
    private float dialogueTime;
    private readonly float defaultSentenceTime = 6.0f;
    private Queue<string> dialogueQueue;
    private Canvas dialogueCanvas;
    private TextMeshProUGUI dialogueText;

    public bool isTalking = false;
    InteractionManager interactionManager;
    EndDialogueTrigger finalDialogue;

    private void Start()
    {
        dialogueQueue = new Queue<string>();

        finalText.text = "";
    }

    public void StartDialogue(Dialogue _dialogue, Canvas _canvas, AudioClip _audio, InteractionManager _manager)
    {
        if (_manager != null)
        {
            if (_manager.isNoticed)
            {
                _manager.DisplayNotice(_manager.noticeText[_manager.LevelIndex]);
                return;
            }
            interactionManager = _manager;
        }

        if (_dialogue == null) return;

        //isPlayCompleted = false;
        if (VO != null && !VO.isPlaying) VO.PlayOneShot(_audio);

        foreach (string _sentence in _dialogue.sentences)
        {
            dialogueQueue.Enqueue(_sentence);
        }
        dialogueCanvas = _canvas;
        dialogueTime = _audio.length / _dialogue.sentences.Length;
        NextSentence();

        isTalking = true;
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

    public void NextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            //isPlayCompleted = true;
            EndDialogue();

            if (interactionManager != null)
                interactionManager.PlayAudio();
            else if (finalDialogue != null)
                StartCoroutine(NextFinalDialogue());
        }
        else
        {
            string sentence = dialogueQueue.Dequeue();
            StopAllCoroutines();
            StartCoroutine(Type(sentence));
        }
    }

    private IEnumerator Type(string sentence)
    {
        if (dialogueCanvas != null)
        {
            dialogueText = dialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
            dialogueText.text = "";
            dialogueCanvas.enabled = true;
            //dialogueText.text += sentence;

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
            finalText.text = sentence;
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

    public void EndDialogue()
    {
        CleanText();
        VO.Stop();
    }

    private void CleanText()
    {
        StopAllCoroutines();
        dialogueQueue.Clear();
        if (dialogueCanvas != null)
        {
            dialogueCanvas.enabled = false;
            dialogueCanvas = null;
            dialogueText.text = null;
        }
        else
        {
            finalText.text = null;
        }
    }
}