using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager_D3 : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameTag;
    [SerializeField] GameObject player;

    public float typeSpeed;
    public float dialogueTime;
    float dialogueTimer = 0f;

    public float maxRotationAngle;
    float rotationY;

    private Queue<string> dialogueQueue;
    bool isTyped = false;
    bool dialogueShown = false;


    private void Start()
    {
        nameTag.text = "";
        dialogueText.text = "";
        canvas.enabled = false;
        dialogueQueue = new Queue<string>();
    }

    private void Update()
    {
        if (dialogueText.text != "" || isTyped)
        {
            if
                ((player.transform.rotation.y - rotationY < -maxRotationAngle)
                || (player.transform.rotation.y - rotationY > maxRotationAngle))
            {
                EndDialogue();
            }
        }

        if (dialogueShown)
        {
            dialogueTimer += Time.deltaTime;
            if (dialogueTimer >= dialogueTime)
            {
                NextSentence();
                dialogueTimer = 0f;
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueQueue.Clear();
        rotationY = player.transform.rotation.y;

        foreach (string sentence in dialogue.sentences)
        {
            nameTag.text = dialogue.name;
            dialogueQueue.Enqueue(sentence);
        }

        NextSentence();
    }

    public void NextSentence()
    {
        dialogueShown = false;
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            string sentence = dialogueQueue.Dequeue();
            StopAllCoroutines();
            StartCoroutine(Type(sentence));
        }
    }

    IEnumerator Type(string sentence)
    {
        dialogueText.text = "";
        canvas.enabled = true;
        isTyped = true;

        foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyped = false;
        dialogueShown = true;
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        dialogueQueue.Clear();

        canvas.enabled = false;
        dialogueText.text = "";

        dialogueTimer = 0f;
        isTyped = false;
        dialogueShown = false;
    }
}