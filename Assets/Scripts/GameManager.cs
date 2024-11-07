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

    [Header("EndingScene")]
    public AudioClip welcomeAudio;

    private int completedLevelCount = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        transitionNotice.text = "";

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            passthroughLayers.textureOpacity = 1;
            //if (changeSceneAudio != null && dialogueManager.VO != null)
            //    dialogueManager.VO.PlayOneShot(changeSceneAudio);

            if (transitionMusic.isPlaying) transitionMusic.Stop();
            if (dialogueManager.VO != null) dialogueManager.VO.PlayOneShot(welcomeAudio);
        }
    }
    private void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.Two) && SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("Press B");
            EndLevel();
        }
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

    public void Test()
    {
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
        transitionMusic.Play();
        //StartCoroutine(TypeEndNotice(endNoticeText));

        if (dialogueManager.VO != null) dialogueManager.VO.PlayOneShot(transitionTriggerAudio);    
    }

    private IEnumerator TypeEndNotice(string _text)
    {
        if (transitionNotice.text != null) transitionNotice.text = "";
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
        float elapsedTime = 0f;
        float startValue = passthroughLayers.textureOpacity;
        float endValue = (SceneManager.GetActiveScene().buildIndex == 0) ? 0f : 1f;

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
        if (SceneManager.GetActiveScene().buildIndex == 1) triggerInterval += triggerInterval;

        transitionNotice.text = "";

        sceneTransition.SetBool("IsEyeClosed", true);

        yield return new WaitForSeconds(triggerInterval);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        StartCoroutine(ChangePassThroughOpacity());

        yield return new WaitForSeconds(triggerInterval);

        sceneTransition.SetBool("IsEyeClosed", false);
    }
}
