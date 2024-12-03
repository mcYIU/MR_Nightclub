using System.Collections;
using TMPro;
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
        public GameLevelTrigger triggerPoint;
        public float interval;
        public string noticeText;
        public TextMeshProUGUI noticeUI;
        public AudioClip transitionMusic;
        public AudioClip guideAudio;
    }

    [Serializable]
    private struct PassthroughConfig
    {
        public OVRPassthroughLayer layers;
        public float fadeDuration;
    }

    public static GameManager Instance { get; private set; }
    public static bool IsStarted { get; set; }
    public static bool IsCompleted { get; private set; }

    [Header("Configurations")]
    [SerializeField] private InteractionConfig interactionConfig;
    [SerializeField] private TransitionConfig transitionConfig;
    [SerializeField] private PassthroughConfig passthroughConfig;

    private int gameSceneIndex;
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
        transitionConfig.noticeUI.text = null;
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        IsCompleted = true;
        DialogueManager.EndDialogue();

        Instance.StartCoroutine(Instance.ChangePassThroughOpacity());
        Instance.DisplayTransitionNotice();
    }

    private void DisplayTransitionNotice()
    {
        transitionConfig.noticeUI.text = transitionConfig.noticeText;
        MusicManager.PlayMusic(transitionConfig.transitionMusic);

        DialogueManager.OverrideSetAudio(transitionConfig.guideAudio, true);
    }

    #endregion

    #region Scene Management

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameSceneIndex = scene.buildIndex;
    }

    public static void ChangeToNextScene()
    {
        Instance.StartCoroutine(Instance.PerformSceneTransition());
    }

    private IEnumerator PerformSceneTransition()
    {
        HandleTransitionMusic();
        ClearTransitionNotice();

        yield return ExecuteSceneChange();
    }

    private void HandleTransitionMusic()
    {
        if (gameSceneIndex != 0)
        {
            transitionConfig.interval++;

            MusicManager.StopMusic();
        }
    }

    private void ClearTransitionNotice()
    {
        transitionConfig.noticeUI.text = null;
    }

    private IEnumerator ExecuteSceneChange()
    {
        FaderController.FadeOut();
        yield return new WaitForSeconds(transitionConfig.interval);

        SceneManager.LoadScene(gameSceneIndex + 1);
        yield return new WaitForSeconds(transitionConfig.interval);

        FaderController.FadeIn();
    }

    #endregion

    #region Visual Effects

    private IEnumerator ChangePassThroughOpacity()
    {
        if (gameSceneIndex != 0) yield break;

        float startValue = passthroughConfig.layers.textureOpacity;
        float elapsedTime = 0f;

        while (elapsedTime < passthroughConfig.fadeDuration)
        {
            passthroughConfig.layers.textureOpacity = Mathf.Lerp(
                startValue,
                FADE_END_VALUE,
                elapsedTime / passthroughConfig.fadeDuration
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        passthroughConfig.layers.textureOpacity = FADE_END_VALUE;
    }

    private IEnumerator TypeEndNotice(string text)
    {
        if (transitionConfig.noticeUI.text != null)
        {
            transitionConfig.noticeUI.text = null;
        }

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '.')
            {
                transitionConfig.noticeUI.text += "\n";
            }
            else
            {
                transitionConfig.noticeUI.text += text[i];
            }
        }

        yield return null;
    }

    #endregion

    private void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.Two) && gameSceneIndex == 0)
        {
            EndLevel();
        }
    }
}