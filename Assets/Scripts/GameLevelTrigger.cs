using UnityEngine;

public class GameLevelTrigger : MonoBehaviour
{
    public ParticleSystem[] customParticles;
    public ParticleSystem mainParticle;
    public Color initialColor;
    public Color endColor;

    private GameManager gameManager;
    private CharacterTrailController trailController;
    private Collider triggerCollider;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        trailController = FindAnyObjectByType<CharacterTrailController>();

        triggerCollider = GetComponent<Collider>();

        EnableTriggerPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.isStarted)
            gameManager.ChangeToNextScene();

        else
            trailController.ResetTrails();

        DisableTriggerPoint();
        if(!gameManager.isStarted) gameManager.isStarted = true;
    }

    public void EnableTriggerPoint()
    {
        for (int i = 0; i < customParticles.Length; i++)
        {
            ParticleSystem.MainModule particleMain = customParticles[i].main;
            particleMain.startColor = new ParticleSystem.MinMaxGradient((!gameManager.isStarted) ? initialColor : endColor);
        }

        if (!triggerCollider.enabled) triggerCollider.enabled = true;
        mainParticle.Play();
    }

    private void DisableTriggerPoint()
    {
        triggerCollider.enabled = false;
        mainParticle.Stop();

        trailController.triggerPointTrail.Stop();
    }
}
