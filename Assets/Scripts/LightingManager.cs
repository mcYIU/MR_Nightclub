using UnityEngine;
using System;
using System.Collections;

public class LightingManager: MonoBehaviour
{
    public Light[] targetLights;
    public Light sceneLight;
    public float targetIntensity;

    private float initialIntensity = 0f;
    private float sceneIntensity = 1f;
    private float transitionDuration = 2f;

    private void Start()
    {
        for (int i = 0; i < targetLights.Length; i++)
        {
            targetLights[i].intensity = initialIntensity;
        }
    }

    public void LightSwitch_Enter(string npc)
    {
        StartCoroutine(SwitchOnOverTime(npc));
    }

    public void LightSwitch_Exit(string npc)
    {
        StartCoroutine(SwitchOffOverTime(npc));
    }

    private IEnumerator SwitchOnOverTime(string npc)
    {
        float elapsedTime = 0f;
        Light _targetLight = Array.Find(targetLights, l => l.name == npc);

        float intensity_sceneLight = sceneLight.intensity;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            _targetLight.intensity = Mathf.Lerp(initialIntensity, targetIntensity, t);
            sceneLight.intensity = Mathf.Lerp(sceneIntensity, 0f, t);
            yield return null;
        }

        _targetLight.intensity = targetIntensity;
        sceneLight.intensity = 0f;
    }

    private IEnumerator SwitchOffOverTime(string npc)
    {
        float elapsedTime = 0f;
        Light _targetLight = Array.Find(targetLights, l => l.name == npc);

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            _targetLight.intensity = Mathf.Lerp(targetIntensity, initialIntensity, t);
            sceneLight.intensity = Mathf.Lerp(0f, sceneIntensity, t);
            yield return null;
        }

        _targetLight.intensity = initialIntensity;
        sceneLight.intensity = sceneIntensity;
    }
}