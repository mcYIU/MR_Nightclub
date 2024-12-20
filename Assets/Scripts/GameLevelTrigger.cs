using UnityEngine;
using System;
using TMPro;

public class GameLevelTrigger : MonoBehaviour
{
    [Serializable]
    private struct VisualElements
    {
        public ParticleSystem visual;
        public GameObject UI;
    }

    [Serializable]
    private struct AudioElements
    {
        public AudioClip audioClip;
    }

    [Serializable]
    private struct TransitionConfig
    {
        public TextMeshProUGUI noticeUI;
        public string[] noticeText;
        public AudioClip[] noticeAudio;
        public AudioClip transitionMusic;
        public int TransitionState;
    }

    [Serializable]
    private struct NPC
    {
        public GameObject gameObject;
        public DialogueTrigger dialogueTrigger;
        public float invokeDelay;
    }

    [SerializeField] private VisualElements visualElements;
    [SerializeField] private AudioElements audioElements;
    [SerializeField] private TransitionConfig transitionConfig;
    [SerializeField] private NPC npc;

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

        Invoke(nameof(EnableTriggerPoint), 1.0f);
    }

    private void InitializeComponents()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private void SetInitialState()
    {
        SetGameObjectState(npc.gameObject, false);
    }

    #endregion

    #region Trigger Handling

    private void OnTriggerEnter(Collider other)
    {
        SetTriggerState(TriggerState.Inactive);
        

        if (GameManager.IsStarted)
        {
            HandleSceneChange();
        }
        else
        {
            HandleGameStart();
        }
    }

    private void HandleSceneChange()
    {
        GameManager.ChangeToNextScene();
    }

    private void HandleGameStart()
    {
        GameManager.IsStarted = true;

        StartInteractionLevel();
        SetAudioElements(false);
        SetTransitionState();
    }

    private void StartInteractionLevel()
    {
        SetGameObjectState(npc.gameObject, true);
        Invoke(nameof(StartNPCDialogue), npc.invokeDelay);
    }

    private void StartNPCDialogue()
    {
        npc.dialogueTrigger.StartDialogue();
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
        SetNoticeDisplay(transitionConfig.TransitionState, isActive);
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

    private void SetParticleState(ParticleSystem particles, bool isActive)
    {
        if (particles != null)
        {
            if (isActive) particles.Play();
            else particles.Stop();
        }
    }

    private void SetGameObjectState(GameObject obj, bool state)
    {
        if (obj != null) obj.SetActive(state);
    }

    private void SetAudioState(AudioClip audio, bool isActive)
    {
        if (isActive)
        {
            SoundEffectManager.PlaySFXLoop(audio);
        }
        else
        {
            SoundEffectManager.StopSFXLoop();
        }
    }

    private void SetNoticeDisplay(int level, bool isActive)
    {
        transitionConfig.noticeUI.text = (isActive) ? transitionConfig.noticeText[level] : string.Empty;
        transitionConfig.noticeUI.enabled = isActive;

        AudioClip _clip = (isActive) ? transitionConfig.noticeAudio[level] : null;
        DialogueManager.OverrideSetAudio(_clip);

        if (level > 0 && isActive) MusicManager.PlayMusic(transitionConfig.transitionMusic);
    }

    private void SetTransitionState()
    {
        transitionConfig.TransitionState++;
    }

    #endregion
}