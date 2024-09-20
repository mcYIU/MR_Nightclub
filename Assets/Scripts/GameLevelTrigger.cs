using UnityEngine;

public class GameLevelTrigger : MonoBehaviour
{
    public ParticleSystem startPoint;
    public AudioSource particleSound;
    public Animator NPC_Animator;
    public AudioSource NPC_Wellcome;

    private GameManager gameManager;
    private Collider triggerCollider;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        triggerCollider = GetComponent<Collider>();

        EnableTriggerPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.isStarted)
            gameManager.ChangeToNextScene();
        else
            NPC_Wellcome.Play();

        DisableTriggerPoint();
        if(!gameManager.isStarted) gameManager.isStarted = true;
    }

    public void EnableTriggerPoint()
    {
        triggerCollider.enabled = true;
        if(startPoint != null) startPoint.Play();
        if(particleSound != null) particleSound.Play();
    }

    private void DisableTriggerPoint()
    {
        triggerCollider.enabled = false;
        if (startPoint != null) startPoint.Stop();
        if (particleSound != null) particleSound.Stop();
    }
}
