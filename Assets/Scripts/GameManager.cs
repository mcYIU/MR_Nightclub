using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InteractionManager[] interactionManagers;

    private DialogueTrigger[] dialogueTriggers;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        for (int i = 0; i < interactionManagers.Length; i++)
        {
            dialogueTriggers[i] = interactionManagers[i].gameObject.GetComponent<DialogueTrigger>();
        }
    }

    public void CheckGameState()
    {
        int completedLevelCount = 0;
        for (int i = 0; i < interactionManagers.Length; i++)
        {
            if (interactionManagers[i].levelIndex == interactionManagers[i].ineteractionLayerCount)
            {
                completedLevelCount++;
                if (completedLevelCount == interactionManagers.Length)
                {
                    EndLevel();
                }
                else
                {
                    Debug.Log("Continue");
                }
            }
        }
    }

    private void EndLevel()
    {
        for (int i = 0;i < dialogueTriggers.Length; i++)
        {
            dialogueTriggers[i].StartDialogue(interactionManagers[i].levelIndex);
        }
    }
}
