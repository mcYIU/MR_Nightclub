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
    public int ineteractionLayerCount = 2;
    [HideInInspector] public bool isNoticed = false;

    [Header("Voiceover")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private int levelIndex = 0;
    private List<string> name_interactables_One = new List<string>();
    private readonly int index_One = 1;
    private List<string> name_interactables_Two = new List<string>();
    private readonly int index_Two = 2;

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
        if (name_interactables_One.Contains(obj_name))
        {
            LevelIndex = index_One;
            Debug.Log(levelIndex);
        }
        else if (name_interactables_Two.Contains(obj_name))
        {
            LevelIndex = index_Two;
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

    private IEnumerator EnableInteraction(float intervalDuration)
    {
        yield return new WaitForSeconds(intervalDuration) ;

        DisplayNotice(noticeText[levelIndex]);
        switch (levelIndex)
        {
            case 0:
                for (int i = 0; i < grabInteractables_LV1.Length; i++)
                {
                    grabInteractables_LV1[i].enabled = true;

                }
                for (int i = 0; i < pokeInteractables_LV1.Length; i++)
                {
                    pokeInteractables_LV1[i].enabled = true;
                }
                break;
            case 1:
                for (int i = 0; i < grabInteractables_LV2.Length; i++)
                {
                    grabInteractables_LV2[i].enabled = true;

                }
                for (int i = 0; i < pokeInteractables_LV2.Length; i++)
                {
                    pokeInteractables_LV2[i].enabled = true;
                }
                break;
            default:
                //ResetInteraction();
                break;
        }
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
    }

    public void Test()
    {
        levelIndex = 2;
        gameManager.CheckGameState();
    }
}
