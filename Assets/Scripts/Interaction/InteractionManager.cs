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
    public HandGrabInteractable[] grabInteractables_One;
    public PokeInteractable[] pokeInteractables_One;
    public string notice_One;
    public HandGrabInteractable[] grabInteractables_Two;
    public PokeInteractable[] pokeInteractables_Two;
    public string notice_Two;
    public Canvas noticeCanvas;
    public TextMeshProUGUI notice;
    public float noticeDuration = 7f;
    public int ineteractionLayerCount = 2;
    public AudioSource audioSource;
    public AudioClip[] VO;
    [HideInInspector] public bool isNoticeShown = false;

    //public static IntEvent LevelChangedEvent;
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
    }

    public void ChangeLevelIndex(string obj_name)
    {
        notice.text = "";
        isNoticeShown = false;

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
        audioSource.clip = VO[levelIndex];
        float playTime = VO[levelIndex].length;
        audioSource.Play();

        StartCoroutine(EnableInteraction(playTime));
    }

    private IEnumerator EnableInteraction(float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);

        switch (levelIndex)
        {
            case 0:
                for (int i = 0; i < grabInteractables_One.Length; i++)
                {
                    grabInteractables_One[i].enabled = true;
                    StartCoroutine(DisplayNotice(notice_One));
                }
                for (int i = 0; i < pokeInteractables_One.Length; i++)
                {
                    pokeInteractables_One[i].enabled = true;
                    StartCoroutine(DisplayNotice(notice_One));
                }
                break;
            case 1:
                for (int i = 0; i < grabInteractables_Two.Length; i++)
                {
                    grabInteractables_Two[i].enabled = true;
                    StartCoroutine(DisplayNotice(notice_Two));

                }
                for (int i = 0; i < pokeInteractables_Two.Length; i++)
                {
                    pokeInteractables_Two[i].enabled = true;
                    StartCoroutine(DisplayNotice(notice_Two));
                }
                break;
            default:
                ResetInteraction();
                break;
        }
    }

    private IEnumerator DisplayNotice(string noticeText)
    {
        noticeCanvas.enabled = true;
        notice.text = noticeText;
        isNoticeShown = true;

        yield return new WaitForSeconds(noticeDuration);
        noticeCanvas.enabled = false;
        notice.text = "";
    }

    public void CleanNotice()
    {
        StopAllCoroutines();
        noticeCanvas.enabled = false;
        notice.text = "";
    }

    public void ResetInteraction()
    {
        StopAllCoroutines();

        for (int i = 0; i < grabInteractables_One.Length; i++)
        {
            grabInteractables_One[i].enabled = false;
            name_interactables_One.Add(grabInteractables_One[i].name);
        }

        for (int i = 0; i < pokeInteractables_One.Length; i++)
        {
            pokeInteractables_One[i].enabled = false;
            name_interactables_One.Add(pokeInteractables_One[i].name);
        }

        for (int i = 0; i < grabInteractables_Two.Length; i++)
        {
            grabInteractables_Two[i].enabled = false;
            name_interactables_Two.Add(grabInteractables_Two[i].name);
        }

        for (int i = 0; i < pokeInteractables_Two.Length; i++)
        {
            pokeInteractables_Two[i].enabled = false;
            name_interactables_Two.Add(pokeInteractables_Two[i].name);
        }
    }
}
