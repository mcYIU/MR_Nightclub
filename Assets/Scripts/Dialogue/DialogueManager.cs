using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public AudioSource VO;
    public TextMeshProUGUI finalText;
    public float finalDialogueInterval;
    public Animator crossfade;

    [HideInInspector] public bool isPlayCompleted = false;

    private float dialogueTime;
    private Queue<string> dialogueQueue;
    private GameObject dialogueCanvas;
    private TextMeshProUGUI dialogueText;

    InteractionManager interactionManager;

    private void Start()
    {
        dialogueQueue = new Queue<string>();

        finalText.text = "";
    }

    public void StartDialogue(Dialogue dialogue, GameObject canvas, AudioClip clip, InteractionManager manager)
    {
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

    public void StartFinalDialogue(EndDialogueTrigger _endDialogueTrigger)
    {
        isPlayCompleted = false;

        StartCoroutine(TriggerFinalDialogue(_endDialogueTrigger));
    }

    public void NextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            isPlayCompleted = true;
            EndDialogue();

            if(interactionManager != null)
            {
                if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
                {
                    interactionManager.PlayAudio();
                }
            }
            else
            {
                finalText.text = "";
            }
        }
        else
        {
            string sentence = dialogueQueue.Dequeue();
            StopAllCoroutines();
            StartCoroutine(Type(sentence));
        }
    }

    IEnumerator Type(string sentence)
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

    private IEnumerator TriggerFinalDialogue(EndDialogueTrigger _trigger)
    {
        for (int i = 0; i < _trigger.characters.Length; i++)
        {
            _trigger.characters[i].SetActive(true);

            VO.clip = _trigger.VO_Audio[i];
            PlayVoiceOver();

            foreach (string sentence in _trigger.VO_Text[i].sentences)
            {
                dialogueQueue.Enqueue(sentence);
            }
            dialogueCanvas = null;
            dialogueTime = VO.clip.length / _trigger.VO_Text[i].sentences.Length;
            NextSentence();

            yield return new WaitForSeconds(VO.clip.length);
            crossfade.SetTrigger("CloseEye");

            yield return new WaitForSeconds(finalDialogueInterval);
            _trigger.characters[i].SetActive(false);
            crossfade.SetTrigger("OpenEye");
        }

        isPlayCompleted = true;
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