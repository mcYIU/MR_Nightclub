using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
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
    public HandGrabInteractable[] grabInteractables_LV2;
    public PokeInteractable[] pokeInteractables_LV2;

    [Header("Notice")]
    public string[] noticeText;
    public TextMeshProUGUI interactionNotice;
    public TextMeshProUGUI endNotice;
    public float noticeDuration;
    public int ineteractionLayerCount;
    [HideInInspector] public bool isNoticed = false;

    [Header("Voiceover")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private int levelIndex = 0;
    private List<string> interactablesNames_LV1 = new List<string>();
    private List<string> interactablesNames_LV2 = new List<string>();

    GameManager gameManager;
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
                isNoticed = false;

                if (value < ineteractionLayerCount)
                {
                    dialogueTrigger.StartDialogue(levelIndex);
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
        dialogueTrigger = GetComponent<DialogueTrigger>();

        ResetInteraction();

        interactionNotice.text = "";
        endNotice.text = "";
    }

    public void ChangeLevelIndex(string obj_name)
    {
        if (interactablesNames_LV1.Contains(obj_name))
        {
            LevelIndex = 1;
            Debug.Log(levelIndex);
        }
        else if (interactablesNames_LV2.Contains(obj_name))
        {
            LevelIndex = 2;
            Debug.Log(levelIndex);
        }
    }

    public void PlayAudio()
    {
        if (audioSource != null && !audioSource.isPlaying) audioSource.PlayOneShot(audioClips[levelIndex]);

        if (levelIndex < ineteractionLayerCount)
        {
            float playTime = audioClips[levelIndex].length;
            StartCoroutine(EnableInteraction(playTime));
        }
    }

    public void DisplayNotice(string noticeText)
    {
        if (levelIndex < ineteractionLayerCount)
            interactionNotice.text = noticeText;
        else
            StartCoroutine(TypeEndNotice(noticeText));

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
        interactablesNames_LV1.Clear();
        interactablesNames_LV2.Clear();

        AddandDisableInteractables(grabInteractables_LV1, pokeInteractables_LV1, interactablesNames_LV1);
        AddandDisableInteractables(grabInteractables_LV2, pokeInteractables_LV2, interactablesNames_LV2);
    }

    private IEnumerator TypeEndNotice(string _text)
    {
        int currentIndex = 0;

        while (currentIndex < _text.Length)
        {
            char currentChar = _text[currentIndex];
            if (currentChar == '.')
                endNotice.text += "\n";
            else
                endNotice.text += currentChar;
            currentIndex++;
        }

        yield return null;
    }

    private void AddandDisableInteractables(HandGrabInteractable[] grabObjs, PokeInteractable[] pokeObjs, List<string> interactablesNames)
    {
        if (grabObjs.Length > 0)
            foreach (var obj in grabObjs)
            {
                interactablesNames.Add(obj.name);
                obj.enabled = false;
            }

        if (pokeObjs.Length > 0)
            foreach (var obj in pokeObjs)
            {
                interactablesNames.Add(obj.name);
                obj.enabled = false;
            }
    }

    private void EnableGrabInteractables(HandGrabInteractable[] interactables)
    {
        if (interactables.Length > 0)
            foreach (var interactable in interactables)
            {
                interactable.enabled = true;
            }
    }

    private void EnablePokeInteractables(PokeInteractable[] interactables)
    {
        if (interactables.Length > 0)
            foreach (var interactable in interactables)
            {
                interactable.enabled = true;
            }
    }

    private IEnumerator EnableInteraction(float intervalDuration)
    {
        yield return new WaitForSeconds(intervalDuration);

        DisplayNotice(noticeText[levelIndex]);

        switch (levelIndex)
        {
            case 0:
                EnableGrabInteractables(grabInteractables_LV1);
                EnablePokeInteractables(pokeInteractables_LV1);
                break;
            case 1:
                EnableGrabInteractables(grabInteractables_LV2);
                EnablePokeInteractables(pokeInteractables_LV2);
                break;
            default:
                break;
        }
    }
}
