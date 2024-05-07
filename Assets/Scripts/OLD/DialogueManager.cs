using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typeSpeed;

    [SerializeField] GameObject player;
    [SerializeField] float maxRotationAngle;
    float rotationY;

    [SerializeField] float dialogueTime;
    private Queue<string> dialogueQueue;
    bool isTyped = false;
    bool dialogueShown = false;
    float dialogueTimer = 0f;

    private void Start()
    {
        dialogueText.text = "";
        dialogueQueue = new Queue<string>();
    }

    private void Update()
    {
        if (dialogueText.text != "")
        {
            if
                ((player.transform.rotation.y - rotationY < -maxRotationAngle)
                || (player.transform.rotation.y - rotationY > maxRotationAngle))
            {
                EndDialogue();
            }
            if (dialogueShown)
            {
                dialogueTimer += Time.deltaTime;
                if (dialogueTimer >= dialogueTime)
                {
                    NextSentence();
                    dialogueShown = false;
                }
            }
            //if (OVRInput.Get(OVRInput.Button.One))
            //{
            //    NextSentence();
            //}
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Speak");
        dialogueQueue.Clear();
        rotationY = player.transform.rotation.y;

        foreach (string sentence in dialogue.sentences)
        {
           dialogueQueue.Enqueue(sentence);
        }

        NextSentence();
    }

    public void NextSentence()
    {
        if(dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        if(!isTyped)
        {
            string sentence = dialogueQueue.Dequeue();
            StopAllCoroutines();
            StartCoroutine(Type(sentence));
        }
        else return;
    }

    IEnumerator Type (string sentence)
    {
        dialogueText.text = "";
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
        dialogueText.text = "";
        isTyped = false;
    }
}