using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [Serializable]
    private struct InteractionConfig
    {
        public InteractionManager[] managers;
    }

    [Serializable]
    private struct TransitionConfig
    {
        public GameLevelTrigger levelTrigger;     
        public OVRPassthroughLayer layers;
        public float fadeDuration;
        public float interval;
    }

    public static GameManager Instance { get; private set; }
    public static bool IsStarted { get; set; }
    public static bool IsCompleted { get; private set; }

    [Header("Configurations")]
    [SerializeField] private InteractionConfig interactionConfig;
    [SerializeField] private TransitionConfig transitionConfig;

    private static int gameSceneIndex;
    private const float FADE_END_VALUE = 0f;

    #region Initialization

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (gameSceneIndex == SceneManager.sceneCountInBuildSettings - 1 && transitionConfig.layers.textureOpacity > 0)
            StartCoroutine(ChangePassThroughOpacity());
    }

    #endregion

    #region Game State Management

    public static void CheckGameState()
    {
        if (!HasInteractionManagers()) return;

        if (AreAllInteractionsCompleted())
        {
            EndLevel();
        }
    }

    private static bool HasInteractionManagers()
    {
        return Instance.interactionConfig.managers.Length > 0;
    }

    private static bool AreAllInteractionsCompleted()
    {
        int completedCount = 0;
        foreach (var manager in Instance.interactionConfig.managers)
        {
            if (manager.LevelIndex == manager.interactionLayers.Length)
            {
                completedCount++;
            }
        }
        return completedCount == Instance.interactionConfig.managers.Length;
    }

    private static void EndLevel()
    {
        if (gameSceneIndex == 0)
        {
            IsCompleted = true;
            DialogueManager.EndDialogue();

            Instance.StartCoroutine(Instance.ChangePassThroughOpacity());
            Instance.EnableLevelTrigger();
        }
    }

    #endregion

    #region Scene Management

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameSceneIndex = scene.buildIndex;
    }

    public static void ChangeToNextScene(int sceneIncrement = 1)
    {
        Instance.StartCoroutine(Instance.PerformSceneTransition(sceneIncrement));
    }

    private IEnumerator PerformSceneTransition(int sceneIncrement = 1)
    {
        HandleTransitionMusic();

        yield return ExecuteSceneChange(sceneIncrement);
    }

    private void HandleTransitionMusic()
    {
        if (gameSceneIndex != 0) 
        {
            MusicManager.StopMusic();

            transitionConfig.interval += 2.0f;
        }   
    }

    private void EnableLevelTrigger()
    {
        transitionConfig.levelTrigger.EnableTriggerPoint();
    }

    private IEnumerator ExecuteSceneChange(int sceneIncrement = 1)
    {
        FaderController.FadeOut();
        yield return new WaitForSeconds(transitionConfig.interval);

        SceneManager.LoadScene(gameSceneIndex + sceneIncrement);
        yield return new WaitForSeconds(transitionConfig.interval);

        FaderController.FadeIn();
    }

    #endregion

    #region Visual Effects

    private IEnumerator ChangePassThroughOpacity()
    {
        float startValue = transitionConfig.layers.textureOpacity;
        float elapsedTime = 0f;

        while (elapsedTime < transitionConfig.fadeDuration)
        {
            transitionConfig.layers.textureOpacity = Mathf.Lerp(
                startValue,
                FADE_END_VALUE,
                elapsedTime / transitionConfig.fadeDuration
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transitionConfig.layers.textureOpacity = FADE_END_VALUE;
    }

    #endregion

    private void Update()
    {
        if (gameSceneIndex == 0 && !GameManager.IsCompleted)
        {
            if (OVRInput.GetUp(OVRInput.Button.One)) EndLevel();  

            if (OVRInput.GetUp(OVRInput.Button.Two)) GameManager.ChangeToNextScene(2);
        }
    }
}