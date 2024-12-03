using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    [Serializable]
    private struct DialogueComponents
    {
        public AudioSource voiceOver;
        public TextMeshProUGUI screenOverlayTextMesh;
        public float dialogueTransitionInterval;
    }

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

    [SerializeField] private DialogueComponents components;
    [SerializeField] private Canvas monologueCanvas;
    [HideInInspector] public DialogueState state;
    private InteractionManager interactionManager;
    private MonologueTrigger monologue;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        //components.screenOverlayTextMesh.text = null;
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

    public static void OverrideSetAudio(AudioClip clip, bool isActive)
    {
        if (isActive)
            Instance.components.voiceOver.PlayOneShot(clip);
        else
            Instance.components.voiceOver.Stop();
    }

    public static void StartDialogue(Dialogue dialogue, Canvas canvas, InteractionManager interaction)
    {
        if (!ValidateDialogueStart(dialogue, canvas, interaction)) return;

        Instance.InitializeDialogue(dialogue, canvas, interaction);
    }

    private static bool ValidateDialogueStart(Dialogue dialogue, Canvas canvas, InteractionManager interaction)
    {
        if (interaction.noticeSystem.isNoticed)
        {
            interaction.DisplayNotice();
            return false;
        }

        return dialogue.sentences != null &&
            dialogue.voiceOverAudio != null &&
            dialogue.characterAudio != null &&
            canvas != null;
    }

    private void InitializeDialogue(Dialogue dialogue, Canvas canvas, InteractionManager interaction)
    {
        interactionManager = interaction;
        state.dialogue = dialogue;     

        if (dialogue.isVoiceOverEnded)
        {
            PlayCharacterAudio(dialogue.characterAudio);
        }
        else
        {
            SetAudio(state.dialogue.voiceOverAudio, true);
            state.canvas = canvas;
            state.displayTime = dialogue.voiceOverAudio.length / dialogue.sentences.Length;
            EnqueueSentences(dialogue.sentences);
        }
    }

    public static void StartMonologue(MonologueTrigger monologueTrigger)
    {
        Instance.monologue = Instance.monologue ?? monologueTrigger;

        Instance.InitializeMonologue(Instance.monologue.dialogueIndex);
        Instance.NextSentence();
    }

    private void InitializeMonologue(int index)
    {
        SetupCharacter(index);

        //PlayFinalAudio(index);
        //EnqueueSentences(monologue.VO_Text[index].sentences);
        //state.canvas = null;

        Instance.InitializeDialogue(Instance.monologue.monologueContent[index].diaogue, monologueCanvas, null);
        //state.displayTime = CalculateDialogueTime(index);
    }

    private void SetupCharacter(int index)
    {
        if (!monologue.monologueContent[index].character.activeSelf)
        {
            monologue.monologueContent[index].character.SetActive(true);
        }
    }

    private void PlayFinalAudio(int index)
    {
        if (state.dialogue.voiceOverAudio != null)
        {
            SetAudio(state.dialogue.voiceOverAudio, true);
        }
    }

    private float CalculateDialogueTime(int index)
    {
        return components.voiceOver.clip != null
            ? components.voiceOver.clip.length / state.dialogue.sentences.Length
            : DialogueState.defaultSentenceTime;
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

    private void PlayCharacterAudio(AudioClip audio)
    {
        SetAudio(audio, true);
        
        StartCoroutine(EnableInteraction(audio.length));
    }

    private void SetAudio(AudioClip audio, bool isActive)
    {
        if (isActive)
        {
            components.voiceOver.clip = audio;
            components.voiceOver.Play();
        }
        else
        {
            components.voiceOver.Stop();
            components.voiceOver.clip = audio;
        }
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

    private void HandleDialogueEnd()
    {
        EndDialogue();

        if (monologue != null)
        {
            StartCoroutine(NextMonologue());
        }
        else
        {
            state.dialogue.isVoiceOverEnded = true;
            PlayCharacterAudio(state.dialogue.characterAudio);
        }
    }

    private void DisplayNextSentence()
    {
        string sentence = state.queue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator EnableInteraction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        interactionManager.EnableInteraction();
    }

    private IEnumerator TypeSentence(string sentence)
    {
        if (state.canvas != monologueCanvas)
        {
            yield return TypeRegularDialogue(sentence);
        }
        else
        {
            TypeFinalDialogue(sentence);
        }

        yield return new WaitForSeconds(state.displayTime);
        NextSentence();
    }

    private IEnumerator TypeRegularDialogue(string sentence)
    {
        state.text = state.canvas.GetComponentInChildren<TextMeshProUGUI>();
        state.text.text = string.Empty;
        state.canvas.enabled = true;

        state.text.text = sentence;
        yield return null;
    }

    private void TypeFinalDialogue(string sentence)
    {
        components.screenOverlayTextMesh.text = sentence;
    }

    private IEnumerator NextMonologue()
    {
        yield return HandleTransition();

        if (monologue.dialogueIndex == monologue.monologueContent.Length)
        {
            monologue.StartChangeSceneDialogue();
        }
        else
        {
            yield return StartNextCharacterDialogue();
        }
    }

    private IEnumerator HandleTransition()
    {
        components.screenOverlayTextMesh.text = null;
        FaderController.FadeOut();
        yield return new WaitForSeconds(components.dialogueTransitionInterval);

        monologue.monologueContent[monologue.dialogueIndex].character.SetActive(false);
        monologue.dialogueIndex++;
        FaderController.FadeIn();
    }

    private IEnumerator StartNextCharacterDialogue()
    {
        monologue.monologueContent[monologue.dialogueIndex].character.SetActive(true);

        yield return new WaitForSeconds(components.dialogueTransitionInterval);

        StartMonologue(monologue);
    }

    public static void EndDialogue()
    {
        Instance.CleanDialogue();

        Instance.SetAudio(null, false);
    }

    private void CleanDialogue()
    {
        StopAllCoroutines();

        if(!state.dialogue.isVoiceOverEnded) CleanRegularDialogue();

        if (state.canvas != monologueCanvas)
        {
            //Instance.CleanRegularDialogue();
        }
        else
        {
            //Instance.CleanRegularDialogue();
            //Instance.components.screenOverlayTextMesh.text = null;
        }
    }

    private void CleanRegularDialogue()
    {
        state.queue.Clear();

        state.canvas.enabled = false;
        state.canvas = null;
        state.text.text = string.Empty;
    }
}