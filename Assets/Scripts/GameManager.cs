using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("FinalDialogue")]
    public InteractionManager[] interactionManagers;
    public DialogueTrigger[] dialogueTriggers;
    public float triggerInterval = 2.0f;
    public AudioSource AS_Clock;
    public AudioSource AS_Shot;

    [Header("Pass Through")]
    public OVRPassthroughLayer passthroughLayers;
    public float passThroughFadeDuration = 5.0f;

    [Header("Notice")]
    public TextMeshProUGUI notice;
    public Image sceneTransition;
    public float readingDuration = 3.0f;

    private LightingManager lightingManager;
    private DialogueManager dialogueManager;
    private CharacterTrailController characterTrailController;
    private int completedLevelCount = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        lightingManager = FindAnyObjectByType<LightingManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        characterTrailController = FindAnyObjectByType<CharacterTrailController>();

        //lightingManager.LightSwitch_Enter(dialogueTriggers[completedLevelCount].name);
    }

    public void CheckGameState()
    {
        completedLevelCount = 0;

        for (int i = 0; i < interactionManagers.Length; i++)
        {
            if (interactionManagers[i].LevelIndex == interactionManagers[i].ineteractionLayerCount)
            {
                completedLevelCount++;
            }
        }

        if (completedLevelCount == interactionManagers.Length)
        {
            EndLevel();
        }
        else
        {
            //lightingManager.LightSwitch_Exit(dialogueTriggers[completedLevelCount - 1].name);
        }

        characterTrailController.ResetTrails();

        if(completedLevelCount != interactionManagers.Length)
        {
            //lightingManager.LightSwitch_Enter(dialogueTriggers[completedLevelCount].name);
        }
    }

    public void FinalDialogue()
    {
        StartCoroutine(TriggerFinalDialogue());
    }

    private void EndLevel()
    {
        dialogueManager.EndDialogue();
        lightingManager.QuickSwitchOffAll();

        AS_Clock.Play();
    }

    private IEnumerator TriggerFinalDialogue()
    {
        lightingManager.QuickSwitchOffAll();
        StartCoroutine(ChangeOpacity());

        for (int i = dialogueTriggers.Length - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(triggerInterval);

            int level = interactionManagers[i].LevelIndex;
            if (dialogueTriggers[i].VO_Audio[level] != null)
            {
                lightingManager.QuickSwitchOn(dialogueTriggers[i].name);
                dialogueTriggers[i].StartFinalDialogue(level);

                yield return new WaitForSeconds(dialogueTriggers[i].VO_Audio[level].length);
                lightingManager.QuickSwitchOffAll();
            }
        }

        yield return new WaitForSeconds(triggerInterval);
        AS_Clock.Stop();
        AS_Shot.Play();

        sceneTransition.enabled = true;
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeOpacity()
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

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(triggerInterval * triggerInterval);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
