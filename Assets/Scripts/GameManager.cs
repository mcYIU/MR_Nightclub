using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isStarted = false;

    [Header("SceneTrigger")]
    public GameLevelTrigger triggerPoint;
    public float triggerInterval;
    public Animator sceneTransition;

    [Header("FinalScene")]
    public InteractionManager[] interactionManagers;
    public DialogueTrigger[] dialogueTriggers;
    public AudioSource endSceneMusic;

    [Header("Pass Through")]
    public OVRPassthroughLayer passthroughLayers;
    public float passThroughFadeDuration;

    private LightingManager lightingManager;
    private DialogueManager dialogueManager;
    private CharacterTrailController characterTrailController;
    private int completedLevelCount = 0;
    private int currentSceneIndex;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        lightingManager = FindAnyObjectByType<LightingManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // set the different script instances in different scenes 
        switch (currentSceneIndex)
        {
            case 0:
                characterTrailController = FindAnyObjectByType<CharacterTrailController>();
                characterTrailController.GoToOrigin();
                break;
            case 1:
                sceneTransition.SetTrigger("OpenEye");

                endSceneMusic.Play();

                EndDialogueTrigger _endDialogueTrigger = FindAnyObjectByType<EndDialogueTrigger>();
                if (dialogueManager != null && _endDialogueTrigger != null)
                    dialogueManager.StartFinalDialogue(_endDialogueTrigger);
                break;
        }
    }

    public void CheckGameState()
    {
        completedLevelCount = 0;

        for (int i = 0; i < interactionManagers.Length; i++)
            // if all the character's interactions are completed
            if (interactionManagers[i].LevelIndex == interactionManagers[i].ineteractionLayerCount)
            {
                completedLevelCount++;
            }

        if (completedLevelCount == interactionManagers.Length)
            EndLevel();
        else
            characterTrailController.ResetTrails();
    }

    public void ChangeToNextScene()
    {
        StartCoroutine(ChangeScene());
    }

    private void EndLevel()
    {
        dialogueManager.EndDialogue();
        lightingManager.QuickSwitchOffAll();

        triggerPoint.EnableTriggerPoint();
        characterTrailController.GoToOrigin();

        StartCoroutine(ChangePassThroughOpacity());

        endSceneMusic.Play();
        //endSceneMusic.playOnAwake = true;
    }

    private IEnumerator ChangePassThroughOpacity()
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
        sceneTransition.SetTrigger("CloseEye");

        yield return new WaitForSeconds(triggerInterval);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
