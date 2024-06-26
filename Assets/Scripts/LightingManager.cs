using UnityEngine;
using System;
using System.Collections;

public class LightingManager: MonoBehaviour
{
    public Light[] spotLights;
    public Light directionalLight;
    public float spotLightIntensity = 50f;
    public float directionalLightIntensity = 2f;

    private float initialIntensity = 0f;
    private float transitionDuration = 2f;

    private void Start()
    {
        for (int i = 0; i < spotLights.Length; i++)
        {
            spotLights[i].intensity = initialIntensity;
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
        Light targetLight = Array.Find(spotLights, l => l.name == npc);

        if(targetLight.intensity != spotLightIntensity)
        {
            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / transitionDuration);
                targetLight.intensity = Mathf.Lerp(initialIntensity, spotLightIntensity, t);
                directionalLight.intensity = Mathf.Lerp(directionalLightIntensity, 0f, t);
                yield return null;
            }
            directionalLight.intensity = 0f;
            targetLight.intensity = spotLightIntensity;
        }   
    }

    private IEnumerator SwitchOffOverTime(string npc)
    {
        float elapsedTime = 0f;
        Light targetLight = Array.Find(spotLights, l => l.name == npc);

        if(targetLight.intensity != initialIntensity)
        {
            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / transitionDuration);
                targetLight.intensity = Mathf.Lerp(spotLightIntensity, initialIntensity, t);
                directionalLight.intensity = Mathf.Lerp(0f, directionalLightIntensity, t);
                yield return null;
            }
            directionalLight.intensity = directionalLightIntensity;
            targetLight.intensity = initialIntensity;
        }
    }
}