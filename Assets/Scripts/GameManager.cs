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
    public DialogueManager dialogueManager;
    public InteractionManager[] interactionManagers;
    public AudioSource endSceneMusic;

    [Header("Passthrough")]
    public OVRPassthroughLayer passthroughLayers;
    public float passThroughFadeDuration;

    private LightingManager lightingManager;
    private CharacterTrailController characterTrailController;
    private int completedLevelCount = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        lightingManager = FindAnyObjectByType<LightingManager>();

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                characterTrailController = FindAnyObjectByType<CharacterTrailController>();
                characterTrailController.GoToOrigin();
                break;
            case 1:
                break;
            case 2:
                passthroughLayers.textureOpacity = 1;
                endSceneMusic.Stop();
                break;
            default:
                break;
        }
    }

    public void CheckGameState()
    {
        completedLevelCount = 0;

        if (interactionManagers.Length > 0)
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
        //lightingManager.QuickSwitchOffAll();

        triggerPoint.EnableTriggerPoint();
        characterTrailController.GoToOrigin();

        StartCoroutine(ChangePassThroughOpacity());

        endSceneMusic.Play();
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
        sceneTransition.SetBool("IsEyeClosed", true);

        yield return new WaitForSeconds(triggerInterval);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        yield return new WaitForSeconds(triggerInterval);

        sceneTransition.SetBool("IsEyeClosed", false);
    }
}
