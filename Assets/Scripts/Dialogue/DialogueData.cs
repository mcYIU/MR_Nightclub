using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("Voice Over")]
    public string[] voiceOverText;
    public AudioClip voiceOverAudio;

    [Header("Character Speech")]
    public string[] characterText;
    public AudioClip characterAudio;

    [HideInInspector] public bool isVoiceOverPlayed;

    public void Reset()
    {
        isVoiceOverPlayed = false;
    }
}
