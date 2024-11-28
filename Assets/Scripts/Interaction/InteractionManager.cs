using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

[Serializable]
public struct InteractionLayer
{
    public Interactable[] interactables;
    public Canvas[] UI;
    public string noticeText;
}

public class InteractionManager : MonoBehaviour
{
    public InteractionLayer[] interactionLayers;
    [SerializeField] public NoticeSystem noticeSystem;
    [SerializeField] private DialogueTrigger dialogueTrigger;

    private int levelIndex;

    [System.Serializable]
    public class NoticeSystem
    {
        public TextMeshProUGUI interactionNotice;
        public TextMeshProUGUI endNotice;
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
        ResetInteraction();
        noticeSystem.Initialize();
    }

    public void ChangeLevelIndex(int index)
    {
        LevelIndex = index;
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
            GameManager.CheckGameState();
        }
    }

    public void DisplayNotice()
    {
        if (levelIndex < interactionLayers.Length)
        {
            noticeSystem.interactionNotice.text = interactionLayers[levelIndex].noticeText;
            noticeSystem.isNoticed = true;

            foreach (var canvas in interactionLayers[levelIndex].UI)
            {
                SetUI(canvas, true);
            }
        }
    }

    public void ResetInteraction()
    {
        for (int i = 0; i < interactionLayers.Length; i++)
        {
            foreach (var interactable in interactionLayers[i].interactables)
            {
                interactable.SetInteraction(false);
                interactable.SetInteractionLevel(i);
            }
        }

    }

    public void EnableInteraction()
    {
        DisplayNotice();

        if (levelIndex < interactionLayers.Length)
        {
            EnableInteractables(levelIndex);
        }
    }

    private void EnableInteractables(int levelIndex)
    {
        foreach (var interactable in interactionLayers[levelIndex].interactables)
        {
            interactable.SetInteraction(true);
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

    public void CleanInteraction()
    {
        noticeSystem.CleanNotices();

        foreach (var interactable in interactionLayers[levelIndex--].interactables)
        {
            interactable.SetUI(false);
        }

        StopAllCoroutines();
    }
}