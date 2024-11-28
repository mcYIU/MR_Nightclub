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
    private class DialogueState
    {
        public Dialogue dialogue;
        public Queue<string> queue;
        public Canvas canvas;
        public TextMeshProUGUI text;
        public float displayTime;
        public const float defaultSentenceTime = 5.0f;
    }

    private static DialogueManager instance;
    public static DialogueManager Instance => instance;

    [SerializeField] private DialogueComponents components;
    private DialogueState state;
    private InteractionManager interactionManager;
    private EndDialogueTrigger finalDialogue;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        components.screenOverlayTextMesh.text = null;
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

    public static void OverrideInstructionAudio(AudioClip clip) { Instance.components.voiceOver.PlayOneShot(clip); }

    public static void StartDialogue(Dialogue dialogue, Canvas canvas, InteractionManager interaction)
    {
        if (!ValidateDialogueStart(dialogue, canvas, interaction)) return;

        Instance.InitializeDialogue(dialogue, canvas, interaction);
        Instance.NextSentence();
    }

    private static bool ValidateDialogueStart(Dialogue dialogue, Canvas canvas, InteractionManager interaction)
    {
        if (interaction?.noticeSystem.isNoticed ?? false)
        {
            interaction.DisplayNotice();
            return false;
        }

        return dialogue.sentences != null &&
            dialogue.voiceOverAudio != null &&
            dialogue.characterAudio != null &&
            canvas != null;
    }

    private void InitializeDialogue(Dialogue dialogue, Canvas canvas,InteractionManager interaction)
    {
        interactionManager = interaction;
        state.dialogue = dialogue;

        if (dialogue.isVoiceOverEnded)
        {
            PlayCharacterAudio(dialogue.characterAudio);
        }
        else
        {
            PlayAudio(state.dialogue.voiceOverAudio);
            state.canvas = canvas;
            EnqueueSentences(dialogue.sentences);
            state.displayTime = dialogue.voiceOverAudio.length / dialogue.sentences.Length;
        }
    }

    public static void StartFinalDialogue(EndDialogueTrigger endDialogue)
    {
        Instance.finalDialogue = Instance.finalDialogue ?? endDialogue;
        int currentIndex = EndDialogueTrigger.dialogueIndex;

        Instance.InitializeFinalDialogue(currentIndex);
        Instance.NextSentence();
    }

    private void InitializeFinalDialogue(int index)
    {
        SetupCharacter(index);
        PlayFinalAudio(index);
        EnqueueSentences(finalDialogue.VO_Text[index].sentences);
        state.canvas = null;
        state.displayTime = CalculateDialogueTime(index);
    }

    private void SetupCharacter(int index)
    {
        if (!finalDialogue.characters[index].activeSelf)
        {
            finalDialogue.characters[index].SetActive(true);
        }
    }

    private void PlayFinalAudio(int index)
    {
        if (finalDialogue.VO_Audio[index] != null)
        {
            components.voiceOver.clip = finalDialogue.VO_Audio[index];
            components.voiceOver.Play();
        }
    }

    private float CalculateDialogueTime(int index)
    {
        return components.voiceOver.clip != null
            ? components.voiceOver.clip.length / finalDialogue.VO_Text[index].sentences.Length
            : DialogueState.defaultSentenceTime;
    }

    private void EnqueueSentences(string[] sentences)
    {
        state.queue.Clear();
        foreach (string sentence in sentences)
        {
            state.queue.Enqueue(sentence);
        }
    }

    private void PlayCharacterAudio(AudioClip audio)
    {
        PlayAudio(audio);

        StartCoorutine(EnableInteraction(audio.length));
    }

    private void StartCoorutine(IEnumerator enumerator)
    {
        throw new NotImplementedException();
    }

    private void PlayAudio(AudioClip audio)
    {
        if (components.voiceOver != null)
        {
            components.voiceOver.PlayOneShot(audio);
        }
    }

    private void NextSentence()
    {
        if (Instance.state.queue.Count == 0)
        {
            Instance.HandleDialogueEnd();
        }
        else
        {
            Instance.DisplayNextSentence();
        }
    }

    private void HandleDialogueEnd()
    {
        EndDialogue();

        if (finalDialogue != null)
        {
            StartCoroutine(NextFinalDialogue());
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
        if (state.canvas != null)
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
        state.text.text = "";
        state.canvas.enabled = true;

        foreach (char c in sentence)
        {
            state.text.text += (c == '.') ? "\n" : c.ToString();
            yield return null;
        }
    }

    private void TypeFinalDialogue(string sentence)
    {
        components.screenOverlayTextMesh.text = sentence;
    }

    private IEnumerator NextFinalDialogue()
    {
        yield return HandleTransition();

        if (EndDialogueTrigger.dialogueIndex == finalDialogue.characters.Length)
        {
            finalDialogue.StartChangeSceneDialogue();
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

        finalDialogue.characters[EndDialogueTrigger.dialogueIndex].SetActive(false);
        EndDialogueTrigger.dialogueIndex++;
        FaderController.FadeIn();
    }

    private IEnumerator StartNextCharacterDialogue()
    {
        finalDialogue.characters[EndDialogueTrigger.dialogueIndex].SetActive(true);
        yield return new WaitForSeconds(components.dialogueTransitionInterval);
        StartFinalDialogue(finalDialogue);
    }

    public static void EndDialogue()
    {
        CleanDialogue();
        Instance.components.voiceOver.Stop();
    }

    private static void CleanDialogue()
    {
        Instance.StopAllCoroutines();
        Instance.state.queue.Clear();

        if (Instance.state.canvas != null)
        {
            Instance.CleanRegularDialogue();
        }
        else
        {
            Instance.components.screenOverlayTextMesh.text = null;
        }
    }

    private void CleanRegularDialogue()
    {
        state.canvas.enabled = false;
        state.canvas = null;
        state.text.text = null;
    }
}