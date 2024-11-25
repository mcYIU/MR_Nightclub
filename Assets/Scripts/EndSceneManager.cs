using System.Collections;
using UnityEngine;

public class EndSceneManager : MonoBehaviour
{
    public AudioClip welcomeAudio;
    public GameObject welcomeCharacter;
    public float audioDelay = 3.0f;

    DialogueManager dialogueManager;
    OVRPassthroughLayer layer;

    void Start()
    {
        layer = FindObjectOfType<OVRPassthroughLayer>();
        if (layer.textureOpacity == 0f) layer.textureOpacity = 1.0f;

        dialogueManager = FindObjectOfType<DialogueManager>();
        StartCoroutine(PlayWelcomeAudio());
    }

    private IEnumerator PlayWelcomeAudio()
    {
        yield return new WaitForSeconds(audioDelay); 

        if (dialogueManager.VO != null) dialogueManager.VO.PlayOneShot(welcomeAudio);

        //yield return new WaitForSeconds(welcomeAudio.length);

        //welcomeCharacter.SetActive(false);
    }
}
