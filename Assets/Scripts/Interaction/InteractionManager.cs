using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

[Serializable]
public struct InteractionLayer
{
    public Interactable[] interactables;
    public string noticeText;
}

public class InteractionManager : MonoBehaviour
{
    public InteractionLayer[] interactionLayers;
    public NoticeSystem noticeSystem;
    [SerializeField] private DialogueTrigger dialogueTrigger;

    private int levelIndex = 0;

    [System.Serializable]
    public class NoticeSystem
    {
        public TextMeshProUGUI interactionNotice;
        public TextMeshProUGUI endNotice;
        [HideInInspector] public bool isNoticed;

        public void Initialize()
        {
            interactionNotice.text = string.Empty;
            //endNotice.text = string.Empty;
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

            Debug.Log(LevelIndex);
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
        Debug.Log(LevelIndex);
    }

    private void HandleLevelChange()
    {
        CleanInteraction();
        noticeSystem.isNoticed = false;

        dialogueTrigger.StartDialogue(LevelIndex);

        if (LevelIndex == interactionLayers.Length)
        {
            //dialogueTrigger.EndDialogue();
            GameManager.CheckGameState();
        }
    }

    public void DisplayNotice()
    {
        if (LevelIndex < interactionLayers.Length)
        {
            noticeSystem.interactionNotice.text = interactionLayers[LevelIndex].noticeText;
            noticeSystem.isNoticed = true;

            foreach (var interactable in interactionLayers[LevelIndex].interactables)
            {
                interactable.SetUI(true);
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

        if (LevelIndex < interactionLayers.Length)
        {
            EnableInteractables(LevelIndex);
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
        if (noticeSystem.isNoticed)
        {
            noticeSystem.CleanNotices();

            for (int i = 0; i < interactionLayers.Length; i++)
            {
                foreach (var interactable in interactionLayers[i].interactables)
                {
                    interactable.SetUI(false);
                }
            }
        } 

        StopAllCoroutines();
    }
}