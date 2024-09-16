using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueAutoLoop : MonoBehaviour
{
    public Dialogue dialogue;
    public TextMeshProUGUI TMP;
    public float dialogueTime;

    private void Start()
    {
        PlayDialogue();
    }

    public void PlayDialogue()
    {
        StartCoroutine(Type());
    }

    private IEnumerator Type()
    {
        foreach (string _sentence in dialogue.sentences)
        {
            TMP.text = _sentence;
            yield return new WaitForSeconds(dialogueTime);
        }

        PlayDialogue();
    }
}
