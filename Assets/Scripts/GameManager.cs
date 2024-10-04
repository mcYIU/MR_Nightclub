using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isStarted = false;
    [HideInInspector] public bool isCompleted = false;

    [Header("SceneTrigger")]
    public GameLevelTrigger triggerPoint;
    public float triggerInterval;
    public Animator sceneTransition;

    [Header("FinalScene")]
    public AudioClip endAudio;
    public string endNoticeText;
    public TextMeshProUGUI endNotice;
    public DialogueManager dialogueManager;
    public InteractionManager[] interactionManagers;
    public AudioSource endSceneMusic;

    [Header("Passthrough")]
    public OVRPassthroughLayer passthroughLayers;
    public float passThroughFadeDuration;

    private int completedLevelCount = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        endNotice.text = "";

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.Log("End");
            passthroughLayers.textureOpacity = 1;
            endSceneMusic.Stop();
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

        //StartCoroutine(TypeEndNotice(endNoticeText));
        endSceneMusic.Play();
        if(endAudio != null && dialogueManager.VO != null) dialogueManager.VO.PlayOneShot(endAudio);    
    }

    private IEnumerator TypeEndNotice(string _text)
    {
        if (endNotice.text != null) endNotice.text = "";
        int currentIndex = 0;

        while (currentIndex < _text.Length)
        {
            char currentChar = _text[currentIndex];
            if (currentChar == '.')
                endNotice.text += "\n";
            else
                endNotice.text += currentChar;
            currentIndex++;
        }

        yield return null;
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
        endNotice.text = "";

        sceneTransition.SetBool("IsEyeClosed", true);

        yield return new WaitForSeconds(triggerInterval);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        yield return new WaitForSeconds(triggerInterval);

        sceneTransition.SetBool("IsEyeClosed", false);

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            passthroughLayers.textureOpacity = 1;
    }
}
