using System.Collections;
using UnityEngine;

public class CharacterTrailController : MonoBehaviour
{
    public ParticleSystem[] characterTrails;
    public AudioSource tracingSound;
    public Transform playerTransform;
    public float speed;
    public float stopParticleDistance;

    public void ResetTrails()
    {
        for (int i = 0; i < characterTrails.Length; i++)
        {
            characterTrails[i].transform.parent.TryGetComponent<InteractionManager>(out InteractionManager interactionManager);
            if (interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
            {
                StartCoroutine(PlayTrail(characterTrails[i]));
            }
        }
    }

    private IEnumerator PlayTrail(ParticleSystem particle)
    {
        Transform startPoint = particle.transform.parent;
        Transform endPoint = playerTransform;

        while (Vector3.Distance(startPoint.position, endPoint.position) > stopParticleDistance)
        {
            float journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
            float journeyTime = journeyLength / speed;
            float startTime = Time.time;

            while (Time.time < startTime + journeyTime)
            {
                if (!particle.isPlaying)
                {
                    if (particle != null) particle.Play();
                    if (tracingSound != null) tracingSound.Play();
                }

                float distanceCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distanceCovered / journeyLength;
                particle.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
                yield return null;
            }

            Transform tempValue = startPoint;
            startPoint = endPoint;
            endPoint = tempValue;
        }

        StopAllTrails();
    }

    private void StopAllTrails()
    {
        if (tracingSound != null) tracingSound.Stop();

        for (int i = 0; i < characterTrails.Length; i++)
        {
            characterTrails[i].Stop();
        }

        StopAllCoroutines();
    }
}
