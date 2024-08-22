using UnityEngine;

public class StartGameTrigger : MonoBehaviour
{
    public ParticleSystem[] customParticles;
    public Color initialColor;
    public Color endColor;

    private GameManager gameManager;
    private CharacterTrailController trailController;
    private ParticleSystem triggerPoint;
    private Collider triggerCollider;
    private bool isStarted = false;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        trailController = FindAnyObjectByType<CharacterTrailController>();
        triggerPoint = GetComponent<ParticleSystem>();
        triggerCollider = GetComponent<Collider>();

        EnableTriggerPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isStarted)
            gameManager.FinalDialogue();       
        else 
            trailController.ResetTrails();

        DisableTriggerPoint();
        if(!isStarted) isStarted = true;
    }

    public void EnableTriggerPoint()
    {
        for (int i = 0; i < customParticles.Length; i++)
        {
            ParticleSystem.MainModule particleMain = customParticles[i].main;
            particleMain.startColor = (!isStarted) ? initialColor : endColor;
        }

        if (!triggerCollider.isTrigger) triggerCollider.isTrigger = true;
        triggerPoint.Play();
    }

    private void DisableTriggerPoint()
    {
        triggerCollider.isTrigger = false;
        triggerPoint.Stop();
    }
}
