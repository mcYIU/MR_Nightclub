using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public AudioSource VO;
    [HideInInspector] public bool isPlayCompleted = false;

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
        if(manager.isNoticed)
        {
            manager.DisplayNotice(manager.noticeText[manager.LevelIndex]);
            return;
        }
        if(dialogue == null)
        {
            return;
        }
        else
        {
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
    }

    public void NextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            isPlayCompleted = true;
            EndDialogue();
            if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
            {
                interactionManager.PlayAudio();
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

        yield return new WaitForSeconds(dialogueTime);
        NextSentence();
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
        if (dialogueCanvas.activeSelf)
        {
            dialogueCanvas.SetActive(false);
            dialogueText.text = "";
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