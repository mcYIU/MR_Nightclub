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
    //public string[] endingText;
    public TextMeshProUGUI notice;
    public Image sceneTransition;
    public float readingDuration = 3.0f;

    [Header("Trace")]
    public ParticleSystem particle;
    public AudioSource traceSound;
    public Transform playerTransform;
    public Transform endPosition;
    public float speed;
    public float stayDuration;
    public float stopDistance;
    private Transform nextCharacterTransform;

    private LightingManager lightingManager;
    private DialogueManager dialogueManager;
    private int completedLevelCount = 0;

    //private bool isTyping = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        lightingManager = FindAnyObjectByType<LightingManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();

        EnableDialogueCharacter(completedLevelCount);
        TraceToNextCharacter();
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
            EnableDialogueCharacter(completedLevelCount);
        }

        TraceToNextCharacter();
    }

    public void FinalDialogue()
    {
        StartCoroutine(TriggerFinalDialogue());
    }

    private void EnableDialogueCharacter(int level)
    {
        for (int i = 0; i < dialogueTriggers.Length; i++)
        {
            dialogueTriggers[i].canTalk = (i == level) ? true : false;
        }
    }

    private void TraceToNextCharacter()
    {
        StopCoroutine(ParticleMove());

        nextCharacterTransform = (completedLevelCount == interactionManagers.Length)? endPosition : dialogueTriggers[completedLevelCount].transform;

        StartCoroutine(ParticleMove());
    }

    private void EndLevel()
    {
        dialogueManager.EndDialogue();
        lightingManager.QuickSwitchOffAll();

        AS_Clock.Play();
    }

    IEnumerator ParticleMove()
    {
        Transform startPoint = playerTransform;
        Transform endPoint = nextCharacterTransform;

        while ((completedLevelCount == interactionManagers.Length)? 
            Vector3.Distance(playerTransform.position, nextCharacterTransform.position) > stopDistance : dialogueTriggers[completedLevelCount].isPlayerOut)
        {
            float journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
            float journeyTime = journeyLength / speed;
            float startTime = Time.time;

            while (Time.time < startTime + journeyTime)
            {
                if (!particle.isPlaying)
                {
                    particle.Play();
                    traceSound.Play();
                }

                float distanceCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distanceCovered / journeyLength;
                particle.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
                yield return null;
            }

            yield return new WaitForSeconds(stayDuration);
            Transform tempValue = startPoint;
            startPoint = endPoint;
            endPoint = tempValue;
        }

        particle.Stop();
        traceSound.Stop();

        StopCoroutine(ParticleMove());  
    }

    IEnumerator TriggerFinalDialogue()
    {
        lightingManager.QuickSwitchOffAll();
        StartCoroutine(ChangeOpacity());
        //StartCoroutine(TypeText());

        for (int i = dialogueTriggers.Length - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(triggerInterval);

            int level = interactionManagers[i].LevelIndex;
            if (dialogueTriggers[i].VO_Audio[level] != null)
            {
                lightingManager.QuickSwitchOn(dialogueTriggers[i].name);
                dialogueTriggers[i].StartFinalDialogue(level);
                //dialogueTriggers[i].StartDialogue(interactionManagers[i].LevelIndex);

                yield return new WaitForSeconds(dialogueTriggers[i].VO_Audio[level].length);
                lightingManager.QuickSwitchOffAll();
            }
        }

        //isTyping = false;

        yield return new WaitForSeconds(triggerInterval);
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

    /*IEnumerator TypeText()
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
    }*/

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(triggerInterval * triggerInterval);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
