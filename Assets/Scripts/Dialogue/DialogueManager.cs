using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Text;

public class DialogueManager : MonoBehaviour
{
    [Serializable]
    public class DialogueState
    {
        public Dialogue dialogue;
        public Queue<string> queue = new Queue<string>();
        public Canvas canvas;
        public TextMeshProUGUI text;
        public float displayTime;
        public const float defaultSentenceTime = 5.0f;
    }

    private static DialogueManager instance;
    public static DialogueManager Instance => instance;
    public static bool isTalking = false;

    [Header("Components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Canvas monologueCanvas;
    [SerializeField] private float monologueTransitionInterval;
    [HideInInspector] public DialogueState state;

    private GameObject currentMonologueCharacter;
    private DialogueTrigger dialogueTrigger;
    private MonologueTrigger monologueTrigger;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
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

    public static void OverrideSetAudio(AudioClip clip)
    {
        Instance.SetAudio(clip);
    }

    public static void StartDialogue(Dialogue dialogue, Canvas canvas, DialogueTrigger trigger = null)
    {
        Instance.InitializeDialogue(dialogue, canvas, trigger);
    }

    private void InitializeDialogue(Dialogue dialogue, Canvas canvas, DialogueTrigger trigger = null)
    {
        isTalking = true;

        dialogueTrigger = trigger;
        state.dialogue = dialogue;
        state.canvas = canvas;

        if (state.dialogue.voiceOverText != null && state.dialogue.voiceOverAudio != null && !state.dialogue.isVoiceOverPlayed)
        {
            StartVoiceOver();
        }
        else
        {
            StartCharacterSpeech();
        }
    }

    private void StartVoiceOver()
    {
        LoadDialogueState(state.dialogue.voiceOverText, state.dialogue.voiceOverAudio);
    }

    private void StartCharacterSpeech()
    {
        state.dialogue.isVoiceOverPlayed = true;

        LoadDialogueState(state.dialogue.characterText, state.dialogue.characterAudio);
    }

    public static void StartMonologue(MonologueContent monologue, MonologueTrigger trigger)
    {
        Instance.InitializeMonologue(monologue.diaogue, Instance.monologueCanvas, monologue.character, trigger);
    }

    private void InitializeMonologue(Dialogue dialogue, Canvas canvas, GameObject character, MonologueTrigger trigger)
    {
        if (monologueTrigger == null) monologueTrigger = trigger;

        InitializeDialogue(dialogue, canvas);
        SetupCharacter(character);
    }

    private void SetupCharacter(GameObject character)
    {
        if (!character.activeSelf)
        {
            character.SetActive(true);
            currentMonologueCharacter = character;
        }
        else
        {
            character.SetActive(false);
        }
    }

    private void LoadDialogueState(string[] sentences, AudioClip clip)
    {
        state.displayTime = clip.length / sentences.Length;
        EnqueueSentences(sentences);
        SetAudio(clip);
    }

    private void SetAudio(AudioClip audio = null)
    {
        if (audio != null)
        {
            audioSource.clip = audio;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void EnqueueSentences(string[] sentences)
    {
        state.queue.Clear();

        foreach (string sentence in sentences)
        {
            state.queue.Enqueue(sentence);
        }

        NextSentence();
    }

    private void NextSentence()
    {
        if (state.queue.Count == 0)
        {
            HandleDialogueEnd();
        }
        else
        {
            DisplayNextSentence();
        }
    }

    private void DisplayNextSentence()
    {
        string sentence = state.queue.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        yield return TypeDialogue(sentence);

        yield return new WaitForSeconds(state.displayTime);

        NextSentence();
    }

    private IEnumerator TypeDialogue(string text)
    {
        state.text = state.canvas.GetComponentInChildren<TextMeshProUGUI>();
        state.canvas.enabled = true;

        StringBuilder builder = new StringBuilder();
        foreach (char c in text)
        {
            if (c == '.') builder.Append('\n');
            else builder.Append(c);
        }
        state.text.text = builder.ToString();

        yield return null;
    }

    private void HandleDialogueEnd()
    {
        EndDialogue();

        if (monologueTrigger != null)
        {
            StartCoroutine(HandleMonologueTransition());
            return;
        }

        if (state.dialogue.isVoiceOverPlayed)
        {
            EnableInteraction();
        }
        else
        {
            state.dialogue.isVoiceOverPlayed = true;
            InitializeDialogue(state.dialogue, state.canvas, dialogueTrigger);
        }
    }

    public static void EndDialogue()
    {
        Instance.CleanDialogue();

        Instance.SetAudio(null);
    }

    private void CleanDialogue()
    {
        StopAllCoroutines();

        CleanRegularDialogue();
    }

    private void CleanRegularDialogue()
    {
        state.queue.Clear();

        state.canvas.enabled = false;
        state.text.text = string.Empty;
    }

    private void EnableInteraction()
    {
        if (dialogueTrigger != null)
        {
            dialogueTrigger.EnableInteraction();
        }
        else
        {
            isTalking = false;
        }
    }

    private IEnumerator HandleMonologueTransition()
    {
        FaderController.FadeOut();

        yield return new WaitForSeconds(monologueTransitionInterval);

        SetupCharacter(currentMonologueCharacter);
        monologueTrigger.MonologueIndex++;

        FaderController.FadeIn();
    }
}