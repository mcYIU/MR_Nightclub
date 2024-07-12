using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public AudioSource VO;
    //public AudioSource endingSound;
    [HideInInspector] public bool isDialogueShowing = false;
    [HideInInspector] public bool isAudioPlaying = false;

    private float dialogueTime;
    private Queue<string> dialogueQueue;
    private GameObject dialogueCanvas;
    private TextMeshProUGUI dialogueText;

    InteractionManager interactionManager;

    private void Start()
    {
        dialogueQueue = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, GameObject canvas, AudioClip clip, InteractionManager manager)
    {
        VO.Stop();
        CleanText();
        if(manager != null && manager.isNoticeShown)
        {
            return;
        }
        else
        {
            interactionManager = manager;

            VO.clip = clip;
            VOPlayer();

            foreach (string sentence in dialogue.sentences)
            {
                dialogueQueue.Enqueue(sentence);
            }
            dialogueCanvas = canvas;    
            dialogueTime = clip.length / dialogue.sentences.Length;
            NextSentence();
        }
    }

    public void NextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            interactionManager.PlayAudio();
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
            isDialogueShowing = true;

            dialogueText.text += "<rotate=90>" + sentence;
        }

        yield return new WaitForSeconds(dialogueTime);
        NextSentence();
    }

    public void EndDialogue()
    {
        CleanText();

        VOPlayer();
    }

    private void CleanText()
    {
        StopAllCoroutines();
        dialogueQueue.Clear();
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
            dialogueText.text = "";
        }
        isDialogueShowing = false;
    }

    private void VOPlayer()
    {
        if (!VO.isPlaying && !isAudioPlaying && !interactionManager.isNoticeShown)
        {
            isAudioPlaying = true;
            VO.Play();
        }
        else
        {
            VO.Stop();
            VO.clip = null;
            isAudioPlaying = false;
        }
    }
}