using System;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public InteractionLayer[] interactionLayers;
    public NoticeSystem noticeSystem;
    [SerializeField, Header("Dialogue Trigger")] private DialogueTrigger dialogueTrigger;

    private int levelIndex = 0;

    [Serializable]
    public struct InteractionLayer
    {
        public Interactable[] interactables;
        public string noticeText;
    }

    [Serializable]
    public class NoticeSystem
    {
        public TextMeshProUGUI interactionNotice;
        [HideInInspector] public bool isNoticed;

        public void Initialize()
        {
            interactionNotice.text = string.Empty;
            isNoticed = false;
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
        CleanInteraction();
        noticeSystem.isNoticed = false;

        dialogueTrigger.StartDialogue(LevelIndex);

        if (LevelIndex == interactionLayers.Length)
        {
            GameManager.CheckGameState();
        }
    }

    private void DisplayNotice()
    {
        if (LevelIndex < interactionLayers.Length)
        {
            noticeSystem.interactionNotice.enabled = true;
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
        if (LevelIndex < interactionLayers.Length)
        {
            DisplayNotice();

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

    public void CleanInteraction()
    {
        if (noticeSystem.isNoticed)
        {
            noticeSystem.Initialize();

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