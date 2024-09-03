using UnityEngine;

public class ChangeSceneTrigger : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Finish"))
            gameManager.ChangeToNextScene();
    }
}
