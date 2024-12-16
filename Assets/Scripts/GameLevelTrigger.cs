using UnityEngine;

public class GameLevelTrigger : MonoBehaviour
{
    public ParticleSystem startPoint;
    public GameObject footUI;
    public GameObject textUI;
    public GameObject lights;
    public AudioSource particleSound;
    public GameObject NPC;

    private GameManager gameManager;
    private Collider triggerCollider;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        triggerCollider = GetComponent<Collider>();

        if (NPC != null) NPC.SetActive(false);
        if (lights != null) lights.SetActive(false);

        EnableTriggerPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.isStarted) gameManager.ChangeToNextScene();
        else
        {
            StartFirstScene();
            GameManager.isStarted = true;
        }

        DisableTriggerPoint();
    }

    private void StartFirstScene()
    {
        lights.SetActive(true);

        NPC.SetActive(true);
        NPC.TryGetComponent<DialogueTrigger>(out DialogueTrigger _npcTrigger);
        if (_npcTrigger != null) _npcTrigger.StartDialogue(0);
    }

    public void EnableTriggerPoint()
    {
        triggerCollider.enabled = true;
        if (footUI != null) footUI.SetActive(true);
        if (textUI != null) textUI.SetActive(true);
        if (startPoint != null) startPoint.Play();
        if (particleSound != null) particleSound.Play();
    }

    private void DisableTriggerPoint()
    {
        triggerCollider.enabled = false;
        if (footUI != null) footUI.SetActive(false);
        if (textUI != null) textUI.SetActive(false);
        if (startPoint != null) startPoint.Stop();
        if (particleSound != null) particleSound.Stop();
    }
}
