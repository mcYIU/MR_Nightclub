using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string[] voiceOverText;
    public AudioClip voiceOverAudio;
    public string[] characterText;
    public AudioClip characterAudio;

    [HideInInspector] public bool isVoiceOverPlayed = false;
}