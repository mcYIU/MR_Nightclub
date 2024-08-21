using UnityEngine;

public class FinalDialogueTrigger : MonoBehaviour
{
    GameManager gameManager;
    bool isTalking = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.AS_Clock.isPlaying && !isTalking)
        {
            gameManager.FinalDialogue();
            isTalking = true;
        }
    }
}
