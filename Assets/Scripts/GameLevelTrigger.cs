using UnityEngine;
using System;

public class GameLevelTrigger : MonoBehaviour
{
    [Serializable]
    private struct VisualElements
    {
        public ParticleSystem visual;
        public GameObject UI;
        public GameObject NPC;
    }

    [Serializable]
    private struct AudioElements
    {
        //public AudioSource audioSource;
        public AudioClip audioClip;
    }

    [SerializeField] private VisualElements visualElements;
    [SerializeField] private AudioElements audioElements;
    [SerializeField, Header("Start Scene")] private AudioClip welcomeDialogue;

    private Collider triggerCollider;

    private enum TriggerState
    {
        Active,
        Inactive
    }

    #region Initialization

    private void Start()
    {
        InitializeComponents();
        SetInitialState();
        EnableTriggerPoint();
    }

    private void InitializeComponents()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private void SetInitialState()
    {
        SetGameObjectState(visualElements.NPC, false);
    }

    #endregion

    #region Trigger Handling

    private void OnTriggerEnter(Collider other)
    {
        SetTriggerState(TriggerState.Inactive);

        if (GameManager.IsStarted) 
            HandleGameStarted();
        else 
            HandleFirstStart();
    }

    private void HandleGameStarted()
    {
        GameManager.ChangeToNextScene();
    }

    private void HandleFirstStart()
    {
        StartFirstScene();
        SetAudioElements(false);
        GameManager.IsStarted = true;
    }

    private void StartFirstScene()
    {
        SetGameObjectState(visualElements.NPC, true);
        PlayWelcomeAudio();
    }

    private void PlayWelcomeAudio()
    {
        SetAudioState(welcomeDialogue, true);
    }

    #endregion

    #region Trigger State Management

    public void EnableTriggerPoint()
    {
        SetTriggerState(TriggerState.Active);
    }

    private void SetTriggerState(TriggerState state)
    {
        bool isActive = state == TriggerState.Active;

        SetTriggerComponents(isActive);
        SetVisualElements(isActive);
        SetAudioElements(isActive);
    }

    private void SetTriggerComponents(bool isActive)
    {
        triggerCollider.enabled = isActive;
    }

    private void SetVisualElements(bool isActive)
    {
        SetGameObjectState(visualElements.UI, isActive);
        SetParticleState(visualElements.visual, isActive);
    }

    private void SetAudioElements(bool isActive)
    {
        SetAudioState(audioElements.audioClip, isActive);
    }

    #endregion

    #region Helper Methods

    private void SetGameObjectState(GameObject obj, bool state)
    {
        if (obj != null) obj.SetActive(state);
    }

    private void SetParticleState(ParticleSystem particles, bool isActive)
    {
        if (particles != null)
        {
            if (isActive) particles.Play();
            else particles.Stop();
        }
    }

    private void SetAudioState(AudioClip audio, bool isActive)
    {
        if (isActive)
        {
            MusicManager.PlayMusic(audio);
        }
        else
        {
            MusicManager.StopMusic();
        }
    }

    #endregion
}