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
    public TextMeshProUGUI notice;
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
        notice.text = "";
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

        /*if (levelIndex < ineteractionLayerCount)
        {
            DialogueTrigger trigger = GetComponent<DialogueTrigger>();
            trigger.StartDialogue(levelIndex);
        }
        else
        {
            gameManager.CheckGameState();
        }*/
    }

    public void PlayAudio()
    {
        audioSource.clip = audioClips[levelIndex];
        float playTime = audioClips[levelIndex].length;
        audioSource.Play();

        StartCoroutine(EnableInteraction(playTime));
    }

    public void DisplayNotice(string noticeText)
    {
        notice.text = noticeText;
        isNoticed = true;
    }

    public void CleanNotice()
    {
        notice.text = "";
        StopAllCoroutines();
    }

    public void ResetInteraction()
    {
        interactablesNames_LV1.Clear();
        interactablesNames_LV2.Clear();

        AddandDisableInteractables(grabInteractables_LV1, pokeInteractables_LV1, interactablesNames_LV1);
        AddandDisableInteractables(grabInteractables_LV2, pokeInteractables_LV2, interactablesNames_LV2);
    }

    public void Test()
    {
        levelIndex = 2;
        gameManager.CheckGameState();
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
                /*for (int i = 0; i < grabInteractables_LV1.Length; i++)
                {
                    grabInteractables_LV1[i].enabled = true;

                }
                for (int i = 0; i < pokeInteractables_LV1.Length; i++)
                {
                    pokeInteractables_LV1[i].enabled = true;
                }*/
                EnableGrabInteractables(grabInteractables_LV1);
                EnablePokeInteractables(pokeInteractables_LV1);
                break;
            case 1:
                /*for (int i = 0; i < grabInteractables_LV2.Length; i++)
                {
                    grabInteractables_LV2[i].enabled = true;

                }
                for (int i = 0; i < pokeInteractables_LV2.Length; i++)
                {
                    pokeInteractables_LV2[i].enabled = true;
                }*/
                EnableGrabInteractables(grabInteractables_LV2);
                EnablePokeInteractables(pokeInteractables_LV2);
                break;
            default:
                //ResetInteraction();
                break;
        }
    }

    /*public void ResetInteraction()
    {
        for (int i = 0; i < grabInteractables_LV1.Length; i++)
        {
            grabInteractables_LV1[i].enabled = false;
            name_interactables_One.Add(grabInteractables_LV1[i].name);
        }

        for (int i = 0; i < pokeInteractables_LV1.Length; i++)
        {
            pokeInteractables_LV1[i].enabled = false;
            name_interactables_One.Add(pokeInteractables_LV1[i].name);
        }

        for (int i = 0; i < grabInteractables_LV2.Length; i++)
        {
            grabInteractables_LV2[i].enabled = false;
            name_interactables_Two.Add(grabInteractables_LV2[i].name);
        }

        for (int i = 0; i < pokeInteractables_LV2.Length; i++)
        {
            pokeInteractables_LV2[i].enabled = false;
            name_interactables_Two.Add(pokeInteractables_LV2[i].name);
        }
    }*/
}
