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

    [HideInInspector] public bool isPlayCompleted = false;

    private float dialogueTime;
    private Queue<string> dialogueQueue;
    private GameObject dialogueCanvas;
    private TextMeshProUGUI dialogueText;

    CharacterTrailController trails;
    InteractionManager interactionManager;
    EndDialogueTrigger finalDialogue;

    private void Start()
    {
        trails = FindAnyObjectByType<CharacterTrailController>();

        dialogueQueue = new Queue<string>();

        finalText.text = "";
    }

    public void StartDialogue(Dialogue dialogue, GameObject canvas, AudioClip clip, InteractionManager manager)
    {
        if(trails != null) trails.StopAllTrails();

        if (manager.isNoticed)
        {
            manager.DisplayNotice(manager.noticeText[manager.LevelIndex]);
            return;
        }

        if (dialogue == null) return;

        interactionManager = manager;

        isPlayCompleted = false;
        VO.clip = clip;
        PlayVoiceOver();

        foreach (string sentence in dialogue.sentences)
        {
            dialogueQueue.Enqueue(sentence);
        }
        dialogueCanvas = canvas;
        dialogueTime = clip.length / dialogue.sentences.Length;
        NextSentence();
    }

    public void StartFinalDialogue(EndDialogueTrigger _endDialogue)
    {
        if(finalDialogue == null) finalDialogue = _endDialogue;

        if (!finalDialogue.characters[EndDialogueTrigger.dialogueIndex].activeSelf)
            finalDialogue.characters[EndDialogueTrigger.dialogueIndex].SetActive(true);

        isPlayCompleted = false;
        VO.clip = finalDialogue.VO_Audio[EndDialogueTrigger.dialogueIndex];
        PlayVoiceOver();

        foreach (string sentence in finalDialogue.VO_Text[EndDialogueTrigger.dialogueIndex].sentences)
        {
            dialogueQueue.Enqueue(sentence);
        }
        dialogueCanvas = null;
        dialogueTime = VO.clip.length / finalDialogue.VO_Text[EndDialogueTrigger.dialogueIndex].sentences.Length;
        NextSentence();
    }

    public void NextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            isPlayCompleted = true;

            if (interactionManager != null)
            {
                EndDialogue();

                if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount) interactionManager.PlayAudio();
            }
            else StartCoroutine(Transition());
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
            dialogueCanvas.SetActive(true);

            dialogueText.text += sentence;
        }
        else
        {
            finalText.text = sentence;
        }

        yield return new WaitForSeconds(dialogueTime);
        NextSentence();
    }

    private IEnumerator Transition()
    {
        finalText.text = "";

        crossfade.SetBool("IsEyeClosed", true);

        yield return new WaitForSeconds(dialogueInterval);

        finalDialogue.characters[EndDialogueTrigger.dialogueIndex].SetActive(false);
        EndDialogueTrigger.dialogueIndex++;

        crossfade.SetBool("IsEyeClosed", false);

        if (EndDialogueTrigger.dialogueIndex == finalDialogue.characters.Length)
            finalDialogue.ChangeToNextScene();
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
        PlayVoiceOver();
    }

    private void CleanText()
    {
        StopAllCoroutines();
        dialogueQueue.Clear();
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
            dialogueCanvas = null;
            dialogueText.text = "";
        }
        else
        {
            finalText.text = "";
        }
    }

    private void PlayVoiceOver()
    {
        if (!VO.isPlaying && !isPlayCompleted)
        {
            VO.Play();
        }
        else
        {
            VO.Stop();
        }
    }
}