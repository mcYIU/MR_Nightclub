using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InteractionManager[] interactionManagers;
    public DialogueTrigger[] dialogueTriggers;
    public AudioSource AS_Clock;
    public AudioSource AS_Shot;
    public float triggerInterval = 2.0f;

    [Header("Pass Through")]
    public OVRPassthroughLayer passthroughLayers;
    public float passThroughFadeDuration = 5.0f;

    [Header("Notice")]
    public string[] endingText;
    public TextMeshProUGUI notice;
    public Image sceneTransition;
    public float readingDuration = 3.0f;

    LightingManager lightingManager;
    DialogueManager dialogueManager;
    private bool isTyping = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        lightingManager = FindAnyObjectByType<LightingManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();

        //notice.text = "";
    }

    public void CheckGameState()
    {
        int completedLevelCount = 0;
        for (int i = 0; i < interactionManagers.Length; i++)
        {
            if (interactionManagers[i].LevelIndex == interactionManagers[i].ineteractionLayerCount)
            {
                completedLevelCount++;
                if (completedLevelCount == interactionManagers.Length)
                {
                    EndLevel();
                }
                else
                {
                    Debug.Log("GameContinue");
                }
            }
        }
    }

    private void EndLevel()
    {
        lightingManager.QuickSwitchOffAll();
        AS_Clock.Play();

        dialogueManager.EndDialogue();

        StartCoroutine(TriggerFinalDialogue());
    }


    IEnumerator TriggerFinalDialogue()
    {
        lightingManager.QuickSwitchOffAll();
        StartCoroutine(ChangeOpacity());
        //StartCoroutine(TypeText());

        for (int i = 0; i < dialogueTriggers.Length - 1; i++)
        {
            yield return new WaitForSeconds(triggerInterval);

            if (dialogueTriggers[i].VO_Audio[interactionManagers[i].LevelIndex] != null)
            {
                lightingManager.QuickSwitchOn(dialogueTriggers[i].gameObject.name);
                dialogueTriggers[i].StartFinalDialogue(interactionManagers[i].LevelIndex);
                //dialogueTriggers[i].StartDialogue(interactionManagers[i].LevelIndex);

                yield return new WaitForSeconds(dialogueTriggers[i].VO_Audio[interactionManagers[i].LevelIndex].length);
                lightingManager.QuickSwitchOffAll();
            }
        }

        isTyping = false;

        yield return new WaitForSeconds(triggerInterval * triggerInterval);
        AS_Clock.Stop();
        AS_Shot.Play();

        sceneTransition.enabled = true;
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeOpacity()
    {
        float elapsedTime = 0f;
        float startValue = passthroughLayers.textureOpacity;
        float endValue = 0f;

        while (elapsedTime < passThroughFadeDuration)
        {
            passthroughLayers.textureOpacity = Mathf.Lerp(startValue, endValue, elapsedTime / passThroughFadeDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        passthroughLayers.textureOpacity = endValue;
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSeconds(triggerInterval);
        isTyping = true;

        while (isTyping)
        {
            for (int i = 0; i < endingText.Length; i++)
            {
                notice.text = endingText[i];

                yield return new WaitForSeconds(readingDuration);
            }
        }

        notice.text = "";
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(triggerInterval * triggerInterval);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
