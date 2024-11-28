using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string[] sentences;
    public AudioClip voiceOverAudio;
    public AudioClip characterAudio;
    [HideInInspector] public bool isVoiceOverEnded = false;
}