using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct InteractionLayer
{
    public HandGrabInteractable[] grabInteractables;
    public PokeInteractable[] pokeInteractables;
    public Canvas[] interactionUI;
    public string[] interactableNames;
}

[Serializable]
public class AudioData
{
    public AudioSource source;
    public AudioClip[] clips;
}

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction Layers")]
    public InteractionLayer[] interactionLayers;

    [Header("Notice System")]
    [SerializeField] public NoticeSystem noticeSystem;

    [Header("Audio")]
    [SerializeField] private AudioData audioData;

    private int levelIndex;
    private DialogueTrigger dialogueTrigger;

    [System.Serializable]
    public class NoticeSystem
    {
        public string[] noticeText;
        public TextMeshProUGUI interactionNotice;
        public TextMeshProUGUI endNotice;
        public float noticeDuration;
        [HideInInspector] public bool isNoticed;

        public void Initialize()
        {
            interactionNotice.text = string.Empty;
            endNotice.text = string.Empty;
            isNoticed = false;
        }

        public void CleanNotices()
        {
            interactionNotice.text = string.Empty;
            endNotice.text = string.Empty;
        }
    }

    public int LevelIndex
    {
        get => levelIndex;
        set
        {
            if (levelIndex == value) return;

            levelIndex = value;
            HandleLevelChange();
        }
    }

    private void Start()
    {
        InitializeComponents();
        ResetInteraction();
        noticeSystem.Initialize();
    }

    private void InitializeComponents()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    public void ChangeLevelIndex(string objName)
    {
        for (int i = 0; i < interactionLayers.Length; i++)
        {
            if (Array.Exists(interactionLayers[i].interactableNames, name => name == objName))
            {
                LevelIndex = i + 1;
                return;
            }
        }
    }

    private void HandleLevelChange()
    {
        noticeSystem.CleanNotices();
        noticeSystem.isNoticed = false;

        if (levelIndex < interactionLayers.Length)
        {
            dialogueTrigger.StartDialogue(levelIndex);
        }
        else
        {
            dialogueTrigger.EndDialogue();
            PlayAudio();
            GameManager.CheckGameState();
        }
    }

    public void DisplayNotice(string noticeText)
    {
        if (levelIndex < interactionLayers.Length)
        {
            noticeSystem.interactionNotice.text = noticeText;
        }
        else
        {
            StartCoroutine(TypeEndNotice(noticeText));
        }
        noticeSystem.isNoticed = true;
    }

    public void ResetInteraction()
    {
        foreach (var layer in interactionLayers)
        {
            DisableUI(layer.interactionUI);
            InitializeInteractableNames(ref layer);
            DisableInteractables(layer);
        }
    }

    private void InitializeInteractableNames(ref InteractionLayer layer)
    {
        int totalCount = layer.grabInteractables.Length + layer.pokeInteractables.Length;
        if (totalCount > 0)
        {
            layer.interactableNames = new string[totalCount];
        }
    }

    private void DisableInteractables(InteractionLayer layer)
    {
        int index = 0;

        foreach (var obj in layer.grabInteractables)
        {
            layer.interactableNames[index++] = obj.name;
            obj.enabled = false;
        }

        foreach (var obj in layer.pokeInteractables)
        {
            layer.interactableNames[index++] = obj.name;
            obj.enabled = false;
        }
    }

    private void EnableInteractables(InteractionLayer layer)
    {
        foreach (var interactable in layer.grabInteractables)
        {
            interactable.enabled = true;
        }

        foreach (var interactable in layer.pokeInteractables)
        {
            interactable.enabled = true;
        }

        foreach (var ui in layer.interactionUI)
        {
            ui.enabled = true;
        }
    }

    private IEnumerator TypeEndNotice(string text)
    {
        StringBuilder builder = new StringBuilder();

        foreach (char c in text)
        {
            if (c == '.')
            {
                builder.Append('\n');
            }
            else
            {
                builder.Append(c);
            }
            noticeSystem.endNotice.text = builder.ToString();
            yield return null;
        }
    }

    private IEnumerator EnableInteraction(float intervalDuration)
    {
        yield return new WaitForSeconds(intervalDuration);

        DisplayNotice(noticeSystem.noticeText[levelIndex]);

        if (levelIndex < interactionLayers.Length)
        {
            EnableInteractables(interactionLayers[levelIndex]);
        }
    }

    private IEnumerator DelayPlayAudio()
    {
        yield return new WaitForSeconds(1.0f);

        if (audioData.source != null && !audioData.source.isPlaying)
        {
            audioData.source.PlayOneShot(audioData.clips[levelIndex]);
        }

        if (levelIndex < interactionLayers.Length)
        {
            float playTime = audioData.clips[levelIndex].length;
            StartCoroutine(EnableInteraction(playTime));
        }
    }

    private void DisableUI(Canvas[] interactionUI)
    {
        foreach (var icon in interactionUI)
        {
            icon.enabled = false;
        }
    }

    public void PlayAudio() => StartCoroutine(DelayPlayAudio());

    public void CleanNotice()
    {
        noticeSystem.CleanNotices();
        audioData.source.Stop();
        StopAllCoroutines();
    }
}