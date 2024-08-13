using UnityEngine;

public class FinalDialogueTrigger : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameManager.AS_Clock.isPlaying)
        {
            gameManager.FinalDialogue();
        }
    }
}
