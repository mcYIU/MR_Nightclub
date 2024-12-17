using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct MonologueContent
{
    public Dialogue diaogue;
    public GameObject character;
}

public class MonologueTrigger : MonoBehaviour
{
    private int monologueIndex = 0;
    private const float startTime = 3.0f;

    [Header("Character Monologue")]
    public MonologueContent[] monologues;

    [Header("End Text")]
    [SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private float endDuration;

    public int MonologueIndex
    {
        get => monologueIndex;
        set
        {
            if (monologueIndex == value) return;

            monologueIndex = value;
            StartMonologue();

            Debug.Log(monologueIndex);
        }
    }

    private void Start()
    {
        if (TMP != null) TMP.enabled = false;

        Invoke(nameof(StartMonologue), startTime);
    }

    private void StartMonologue()
    {
        if (MonologueIndex == monologues.Length)
        {
            StartCoroutine(DisplayEndText());
        }
        else
        {
            DialogueManager.StartMonologue(monologues[MonologueIndex], this);
        }
    }

    private IEnumerator DisplayEndText()
    {
        TMP.enabled = true;

        yield return new WaitForSeconds(endDuration);

        GameManager.ChangeToNextScene();
    }
}
