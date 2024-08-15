using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightingManager: MonoBehaviour
{
    public Light[] spotLights;
    public Light directionalLight;
    public float spotLightIntensity;

    readonly private float initialIntensity_SpotLight = 0f;
    private float initialIntensity_DirectionalLight;
    readonly private float transitionDuration = 1.0f;

    private Dictionary<string, Light> lightDictionary = new Dictionary<string, Light>();

    private void Start()
    {
        initialIntensity_DirectionalLight = directionalLight.intensity;

        foreach (Light light in spotLights)
        {
            light.intensity = initialIntensity_SpotLight;
            lightDictionary.Add(light.name, light);
        }

        /*for (int i = 0; i < spotLights.Length; i++)
        {
            spotLights[i].intensity = initialIntensity_SpotLight;
        }*/
    }

    public void LightSwitch_Enter(string npc)
    {
        StartCoroutine(SwitchOnOverTime(npc));
    }

    public void LightSwitch_Exit(string npc)
    {
        StartCoroutine(SwitchOffOverTime(npc));
    }

    public void QuickSwitchOn(string npc)
    {
        if(lightDictionary.ContainsKey(npc))
        {
            //Light targetLight = Array.Find(spotLights, l => l.name == npc);          
            Light targetLight = lightDictionary[npc];
            AudioSource sfx = targetLight.GetComponentInChildren<AudioSource>();
            if (targetLight.intensity != spotLightIntensity)
            {
                sfx.Play();
                targetLight.intensity = spotLightIntensity;
            }
        }
    }

    public void QuickSwitchOffAll()
    {
        directionalLight.intensity = 0f;

        foreach (Light light in spotLights)
        {
            if (light.intensity != initialIntensity_SpotLight)
            {
                AudioSource sfx = light.GetComponentInChildren<AudioSource>();
                sfx.Play();
                light.intensity = initialIntensity_SpotLight;
            }
        }

        /*for (int i = 0; i < spotLights.Length; i++)
        {
            if (spotLights[i].intensity != initialIntensity_SpotLight)
            {
                AudioSource sfx = spotLights[i].GetComponentInChildren<AudioSource>();
                sfx.Play();
                spotLights[i].intensity = initialIntensity_SpotLight;
            }
        }*/
    }

    private IEnumerator SwitchOnOverTime(string npc)
    {
        if (lightDictionary.ContainsKey(npc))
        {
            float elapsedTime = 0f;
            Light targetLight = lightDictionary[npc];
            float _lightIntensity_SL = targetLight.intensity;
            //Light targetLight = Array.Find(spotLights, l => l.name == npc);
            //float _lightIntensity_SL = targetLight.intensity;
            float _lightIntensity_DL = directionalLight.intensity;

            if (targetLight.intensity != spotLightIntensity)
            {
                while (elapsedTime < transitionDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / transitionDuration);
                    targetLight.intensity = Mathf.Lerp(_lightIntensity_SL, spotLightIntensity, t);
                    directionalLight.intensity = Mathf.Lerp(_lightIntensity_DL, 0f, t);
                    yield return null;
                }
                directionalLight.intensity = 0f;
                targetLight.intensity = spotLightIntensity;
            }
        }   
    }

    private IEnumerator SwitchOffOverTime(string npc)
    {
        if (lightDictionary.ContainsKey(npc))
        {
            float elapsedTime = 0f;
            Light targetLight = lightDictionary[npc];
            float _lightIntensity_SL = targetLight.intensity;
            //Light targetLight = Array.Find(spotLights, l => l.name == npc);
            //float _lightIntensity_SL = targetLight.intensity;
            float _lightIntensity_DL = directionalLight.intensity;

            if (targetLight.intensity != initialIntensity_SpotLight)
            {
                while (elapsedTime < transitionDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / transitionDuration);
                    targetLight.intensity = Mathf.Lerp(_lightIntensity_SL, initialIntensity_SpotLight, t);
                    directionalLight.intensity = Mathf.Lerp(_lightIntensity_DL, initialIntensity_DirectionalLight, t);
                    yield return null;
                }
                directionalLight.intensity = initialIntensity_DirectionalLight;
                targetLight.intensity = initialIntensity_SpotLight;
            }
        }      
    }
}