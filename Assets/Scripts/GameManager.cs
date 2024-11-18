using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isStarted = false;
    public static bool isCompleted = false;

    public InteractionManager[] interactionManagers;
    public DialogueManager dialogueManager;

    [Header("SceneTransition")]
    public GameLevelTrigger triggerPoint;
    public Animator sceneTransition;
    public float triggerInterval;
    public string transitionNoticeText;
    public TextMeshProUGUI transitionNotice;
    public AudioClip transitionTriggerAudio;
    public AudioSource transitionMusic;

    [Header("Passthrough")]
    public OVRPassthroughLayer passthroughLayers;
    public float passThroughFadeDuration;

    private static GameManager instance;

    private int completedLevelCount = 0;
    private int gameSceneIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        transitionNotice.text = null;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameSceneIndex = scene.buildIndex;
    }

    private void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.Two) && gameSceneIndex == 0) EndLevel();
    }

    public void CheckGameState()
    {
        completedLevelCount = 0;

        if (interactionManagers.Length > 0)
            for (int i = 0; i < interactionManagers.Length; i++)
                // if all the character's interactions are completed
                if (interactionManagers[i].LevelIndex == InteractionManager.ineteractionLayerCount)
                {
                    completedLevelCount++;
                }

        if (completedLevelCount == interactionManagers.Length)
            EndLevel();
    }

    public void ChangeToNextScene()
    {
        StartCoroutine(ChangeScene());
    }

    private void EndLevel()
    {
        isCompleted = true;

        dialogueManager.EndDialogue();
        triggerPoint.EnableTriggerPoint();

        StartCoroutine(ChangePassThroughOpacity());

        transitionNotice.text = transitionNoticeText;
        transitionMusic.Play();
        if (dialogueManager.VO != null) dialogueManager.VO.PlayOneShot(transitionTriggerAudio);
        //StartCoroutine(TypeEndNotice(endNoticeText));
    }

    private IEnumerator TypeEndNotice(string _text)
    {
        if (transitionNotice.text != null) transitionNotice.text = null;
        int currentIndex = 0;

        while (currentIndex < _text.Length)
        {
            char currentChar = _text[currentIndex];
            if (currentChar == '.')
                transitionNotice.text += "\n";
            else
                transitionNotice.text += currentChar;
            currentIndex++;
        }

        yield return null;
    }

    private IEnumerator ChangePassThroughOpacity()
    {
        if (gameSceneIndex == 0)
        {
            float _startValue = passthroughLayers.textureOpacity;
            float _endValue = 0f;

            float _elapsedTime = 0f;
            while (_elapsedTime < passThroughFadeDuration)
            {
                passthroughLayers.textureOpacity = Mathf.Lerp(_startValue, _endValue, _elapsedTime / passThroughFadeDuration);

                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            passthroughLayers.textureOpacity = _endValue;
        }
    }

    private IEnumerator ChangeScene()
    {
        if (gameSceneIndex != 0) triggerInterval++; 

        transitionNotice.text = null;

        sceneTransition.SetBool("IsEyeClosed", true);

        yield return new WaitForSeconds(triggerInterval);

        SceneManager.LoadScene(gameSceneIndex + 1);

        yield return new WaitForSeconds(triggerInterval);

        sceneTransition.SetBool("IsEyeClosed", false);
    }
}
