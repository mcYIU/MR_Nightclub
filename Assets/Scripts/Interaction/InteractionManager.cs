using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

public class InteractionManager : MonoBehaviour
{
    [Header("Interactables")]
    public HandGrabInteractable[] grabInteractables_LV1;
    public PokeInteractable[] pokeInteractables_LV1;
    public Canvas[] interactionUI_LV1;

    public HandGrabInteractable[] grabInteractables_LV2;
    public PokeInteractable[] pokeInteractables_LV2;
    public Canvas[] interactionUI_LV2;

    [Header("Notice")]
    public string[] noticeText;
    public TextMeshProUGUI interactionNotice;
    public TextMeshProUGUI endNotice;
    public float noticeDuration;
    [HideInInspector] public bool isNoticed = false;

    [Header("Voiceover")]
    public AudioSource audioSource;
    public Dialogue[] dialogues;
    public Canvas canvas;
    public AudioClip[] audioClips;

    private string[] interactablesNames_LV1;
    private string[] interactablesNames_LV2;

    public static int ineteractionLayerCount = 2;
    private int levelIndex = 0;

    //private List<string> interactablesNames_LV1 = new List<string>();
    //private List<string> interactablesNames_LV2 = new List<string>();

    GameManager gameManager;
    DialogueManager dialogueManager;
    DialogueTrigger dialogueTrigger;

    public int LevelIndex
    {
        get { return levelIndex; }
        set
        {
            if (levelIndex != value)
            {
                levelIndex = value;

                CleanNotice();
                dialogueManager.isTalking = false;
                isNoticed = false;

                if (value < ineteractionLayerCount)
                {
                    dialogueTrigger.StartDialogue(LevelIndex);
                }
                else
                {
                    dialogueTrigger.EndDialogue();
                    PlayAudio();
                    gameManager.CheckGameState();
                }
            }
        }
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        dialogueTrigger = GetComponent<DialogueTrigger>();

        ResetInteraction();

        interactionNotice.text = "";
        endNotice.text = "";
    }

    public void ChangeLevelIndex(string _objName)
    {
        //if (interactablesNames_LV1.Contains(_objName))
        if (Array.Exists(interactablesNames_LV1, name => name == _objName))
        {
            LevelIndex = 1;
            Debug.Log(levelIndex);
        }
        //else if (interactablesNames_LV2.Contains(_objName))
        else if (Array.Exists(interactablesNames_LV2, name => name == _objName))
        {
            LevelIndex = 2;
            Debug.Log(levelIndex);
        }
    }

    public void PlayAudio()
    {
        if (LevelIndex < ineteractionLayerCount)
        {
            dialogueManager.StartDialogue(dialogues[LevelIndex], canvas, audioClips[LevelIndex], this);

            float _playTime = audioClips[LevelIndex].length;
            StartCoroutine(EnableInteraction(_playTime));
        }
        else
        {
            dialogueManager.StartDialogue(dialogues[LevelIndex], canvas, audioClips[LevelIndex], null);
        }

        //StartCoroutine(DelayPlayAudio());
    }

    public void DisplayNotice(string _noticeText)
    {
        if (levelIndex < ineteractionLayerCount)
            interactionNotice.text = _noticeText;
        else
            StartCoroutine(TypeEndNotice(_noticeText));

        isNoticed = true;
    }

    public void CleanNotice()
    {
        interactionNotice.text = "";
        endNotice.text = "";
        audioSource.Stop();
        StopAllCoroutines();
    }

    public void ResetInteraction()
    {
        DisableUI(interactionUI_LV1);

        if (grabInteractables_LV1.Length > 0 && pokeInteractables_LV1.Length == 0)
            interactablesNames_LV1 = new string[grabInteractables_LV1.Length];
        else if (pokeInteractables_LV1.Length > 0 && grabInteractables_LV1.Length == 0)
            interactablesNames_LV1 = new string[pokeInteractables_LV1.Length];
        else return;
        AddandDisableInteractables(grabInteractables_LV1, pokeInteractables_LV1, ref interactablesNames_LV1);

        DisableUI(interactionUI_LV2);

        if (grabInteractables_LV2.Length > 0 && pokeInteractables_LV2.Length == 0)
            interactablesNames_LV2 = new string[grabInteractables_LV2.Length];
        else if (pokeInteractables_LV2.Length > 0 && grabInteractables_LV2.Length == 0)
            interactablesNames_LV2 = new string[pokeInteractables_LV2.Length];
        else return;
        AddandDisableInteractables(grabInteractables_LV2, pokeInteractables_LV2, ref interactablesNames_LV2);


        /*interactablesNames_LV1.Clear();
        AddandDisableInteractables(grabInteractables_LV1, pokeInteractables_LV1, interactablesNames_LV1);
        DisableUI(interactionUI_LV1);

        interactablesNames_LV2.Clear();
        AddandDisableInteractables(grabInteractables_LV2, pokeInteractables_LV2, interactablesNames_LV2);
        DisableUI(interactionUI_LV2);*/
    }

    public void DisableUI(Canvas[] _interactionUI)
    {
        if (_interactionUI.Length > 0)
            foreach (var _icon in _interactionUI)
                _icon.enabled = false;
    }

    private IEnumerator TypeEndNotice(string _text)
    {
        int _currentIndex = 0;

        while (_currentIndex < _text.Length)
        {
            char _currentChar = _text[_currentIndex];

            if (_currentChar == '.')
                endNotice.text += "\n";
            else
                endNotice.text += _currentChar;

            _currentIndex++;
        }

        yield return null;
    }

    //private void AddandDisableInteractables(HandGrabInteractable[] _grabObjs, PokeInteractable[] _pokeObjs, List<string> _interactablesNames)
    private void AddandDisableInteractables(HandGrabInteractable[] _grabObjs, PokeInteractable[] _pokeObjs, ref string[] _interactablesNames)
    {
        int _index = 0;

        if (_grabObjs.Length > 0)
            foreach (var _obj in _grabObjs)
            {
                //_interactablesNames.Add(_obj.name);
                _interactablesNames[_index++] = _obj.name;
                _obj.enabled = false;
            }

        if (_pokeObjs.Length > 0)
            foreach (var _obj in _pokeObjs)
            {
                //_interactablesNames.Add(_obj.name);
                _interactablesNames[_index++] = _obj.name;
                _obj.enabled = false;
            }
    }

    private void EnableGrabInteractables(HandGrabInteractable[] _interactables, Canvas[] _interactionUI)
    {
        if (_interactables.Length > 0)
            foreach (var _interactable in _interactables)
                _interactable.enabled = true;

        if (_interactionUI.Length > 0)
            foreach (var _icon in _interactionUI)
                _icon.enabled = true;
    }

    private void EnablePokeInteractables(PokeInteractable[] _interactables, Canvas[] _interactionUI)
    {
        if (_interactables.Length > 0)
            foreach (var _interactable in _interactables)
                _interactable.enabled = true;

        if (_interactionUI.Length > 0)
            foreach (var _icon in _interactionUI)
                _icon.enabled = true;
    }

    private IEnumerator EnableInteraction(float _intervalDuration)
    {
        yield return new WaitForSeconds(_intervalDuration);

        DisplayNotice(noticeText[LevelIndex]);

        switch (LevelIndex)
        {
            case 0:
                EnableGrabInteractables(grabInteractables_LV1, interactionUI_LV1);
                EnablePokeInteractables(pokeInteractables_LV1, interactionUI_LV1);
                break;
            case 1:
                EnableGrabInteractables(grabInteractables_LV2, interactionUI_LV2);
                EnablePokeInteractables(pokeInteractables_LV2, interactionUI_LV2);
                break;
            default:
                break;
        }
    }

    private IEnumerator DelayPlayAudio()
    {
        yield return new WaitForSeconds(1.0f);

        if (audioSource != null && !audioSource.isPlaying) audioSource.PlayOneShot(audioClips[LevelIndex]);

        if (LevelIndex < ineteractionLayerCount)
        {
            float _playTime = audioClips[levelIndex].length;
            StartCoroutine(EnableInteraction(_playTime));
        }
    }
}
