using System.Collections;
using UnityEngine;

public class CharacterTrailController : MonoBehaviour
{
    public ParticleSystem[] characterTrails;
    public ParticleSystem triggerPointTrail;
    public AudioSource tracingSound;
    public Transform playerTransform;
    public float speed;
    public float stopParticleDistance;

    private bool isEnteringOrigin = false;

    public void ResetTrails()
    {
        for (int i = 0; i < characterTrails.Length; i++)
        {
            characterTrails[i].transform.parent.parent.TryGetComponent<InteractionManager>(out InteractionManager interactionManager);
            // Find the characters who have not finished all the interactions
            if (interactionManager.LevelIndex < InteractionManager.ineteractionLayerCount)
            {
                StartCoroutine(PlayTrail(characterTrails[i]));
            }
        }
    }

    public void GoToOrigin()
    {
        isEnteringOrigin = true;

        StartCoroutine(PlayTrail(triggerPointTrail));
    }

    private IEnumerator PlayTrail(ParticleSystem particle)
    {
        Transform startPoint = particle.transform.parent;
        Transform endPoint = playerTransform;

        while (true)
        //while (Vector3.Distance(playerTransform.position, particle.transform.parent.position) > stopParticleDistance)
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

            // Move between two points continuously
            (startPoint, endPoint) = (endPoint, startPoint);
        }

        //StopAllTrails();
    }

    public void StopAllTrails()
    {
        if (tracingSound != null) tracingSound.Stop();

        if (isEnteringOrigin)
        {
            triggerPointTrail.Stop();
            isEnteringOrigin = false;
        }
        else
        {
            for (int i = 0; i < characterTrails.Length; i++)
            {
                characterTrails[i].Stop();
                StopAllCoroutines();
            }
        }
    }
}
