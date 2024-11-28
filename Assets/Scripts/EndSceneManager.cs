using System.Collections;
using UnityEngine;

public class EndSceneManager : MonoBehaviour
{
    public AudioClip welcomeAudio;
    public GameObject welcomeCharacter;
    public float audioDelay = 3.0f;

    OVRPassthroughLayer layer;

    void Start()
    {
        layer = FindObjectOfType<OVRPassthroughLayer>();
        if (layer.textureOpacity == 0f) layer.textureOpacity = 1.0f;

        StartCoroutine(PlayWelcomeAudio());
    }

    private IEnumerator PlayWelcomeAudio()
    {
        yield return new WaitForSeconds(audioDelay);

        DialogueManager.OverrideInstructionAudio(welcomeAudio);

    }
}
