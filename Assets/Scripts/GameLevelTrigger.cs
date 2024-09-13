using UnityEngine;

public class GameLevelTrigger : MonoBehaviour
{
    public ParticleSystem startPoint;
    public Animator NPC_Animator;

    private GameManager gameManager;
    private Collider triggerCollider;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        triggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.isStarted)
            gameManager.ChangeToNextScene();
        else
            Debug.Log("Play NPC Anim");

        DisableTriggerPoint();
        if(!gameManager.isStarted) gameManager.isStarted = true;
    }

    public void EnableTriggerPoint()
    {
        triggerCollider.enabled = true;
        startPoint.Play();
    }

    private void DisableTriggerPoint()
    {
        triggerCollider.enabled = false;
        startPoint.Stop();
    }
}
