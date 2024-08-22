using UnityEngine;

public class StartGameTrigger : MonoBehaviour
{
    CharacterTrailController trailController;

    private void Start()
    {
        trailController = FindAnyObjectByType<CharacterTrailController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        trailController.ResetTrails();

        Destroy(gameObject);
    }

    private void DisableTriggerPoint()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = false;

        ParticleSystem particle = GetComponent<ParticleSystem>();
        particle.Stop();
    }
}
