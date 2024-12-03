using UnityEngine;
using System.Collections;

public class MonologueTrigger : MonoBehaviour
{
    [System.Serializable]
    public struct MonologueContent
    {
        public Dialogue diaogue;
        public GameObject character;
    }

    [Header("Character Monologue")]
    public MonologueContent[] monologueContent;

    [Header("End Visual")]
    [SerializeField] private CanvasGroup textCanvasMask;
    [SerializeField] private ParticleSystem visual;
    [SerializeField] private float duration;

    [HideInInspector] public int dialogueIndex = 0;
    private readonly float startTime = 3.0f;
    private ParticleSystem[] sceneParticles;

    private void Start()
    {
        sceneParticles = FindObjectsOfType<ParticleSystem>();
        if (sceneParticles.Length > 0) foreach (ParticleSystem _particle in sceneParticles) _particle.Stop();

        textCanvasMask.alpha = 0f;
        //if (TMP != null) TMP.enabled = false;

        Invoke(nameof(StartDialogue), startTime);
    }

    public void StartChangeSceneDialogue()
    {
        StartCoroutine(Type());
        //if(gameManager != null) gameManager.transitionMusic.Stop();
        //if (TMP != null) TMP.enabled = true;
        //if (particle != null) particle.Play();
        //if (dialogueManager.VO != null) dialogueManager.VO.PlayOneShot(changeSceneAudio);
    }

    private void StartDialogue()
    {
        DialogueManager.StartMonologue(this);
    }

    private IEnumerator Type()
    {
        textCanvasMask.alpha = 1.0f;
        
        foreach (ParticleSystem _particle in sceneParticles) _particle.Play();

        yield return new WaitForSeconds(duration);

        GameManager.ChangeToNextScene();
    }
}
