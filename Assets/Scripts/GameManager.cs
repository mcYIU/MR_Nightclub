using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InteractionManager[] interactionManagers;
    public DialogueTrigger[] dialogueTriggers;
    //public string startNotice;
    //public Canvas noticeCanvas;
    //public TextMeshProUGUI notice;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CheckGameState()
    {
        int completedLevelCount = 0;
        for (int i = 0; i < interactionManagers.Length; i++)
        {
            if (interactionManagers[i].LevelIndex == interactionManagers[i].ineteractionLayerCount)
            {
                completedLevelCount++;
                if (completedLevelCount == interactionManagers.Length)
                {
                    EndLevel();
                }
                else
                {
                    Debug.Log("GameContinue");
                }
            }
        }
    }

    private void EndLevel()
    {
        for (int i = 0; i < dialogueTriggers.Length; i++)
        {
            dialogueTriggers[i].StartDialogue(interactionManagers[i].LevelIndex);
        }
    }
}
