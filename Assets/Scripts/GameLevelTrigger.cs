using UnityEngine;

public class GameLevelTrigger : MonoBehaviour
{
    public ParticleSystem startPoint;
    public GameObject lights;
    public AudioSource particleSound;
    public GameObject NPC;
    //public Animator NPC_Animator;

    private GameManager gameManager;
    private Collider triggerCollider;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        triggerCollider = GetComponent<Collider>();

        if(NPC != null) NPC.SetActive(false);
        if(lights != null) lights.SetActive(false);

        EnableTriggerPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.isStarted) gameManager.ChangeToNextScene();
        else
        {
            StartFirstScene();
            gameManager.isStarted = true;
        }

        DisableTriggerPoint();
    }

    private void StartFirstScene()
    {
        lights.SetActive(true);

        NPC.SetActive(true);
        NPC.TryGetComponent<AudioSource>(out AudioSource AS_Wellcome);
        if (AS_Wellcome != null) AS_Wellcome.Play();
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
