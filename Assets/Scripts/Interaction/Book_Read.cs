using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Book_Read : MonoBehaviour
{
    public InteractionManager interactionManager;
    public TextMeshProUGUI bookText;
    public float animationDurationOffset;
    public string[] sentences;
    public float typeInterval;
    public float readingDuration;
    public AudioSource sFx_Flip;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        bookText.text = "";
    }

    public void ReadText()
    {
        if(bookText != null)
        {
            StartCoroutine(Type());
        }
    }

    IEnumerator Type()
    {
        yield return new WaitForSeconds(animationDurationOffset);
        for(int i = 0; i < sentences.Length; i++)
        {
            sFx_Flip.Play();

            string textBuffer = null;
            foreach (char c in sentences[i])
            {
                textBuffer += c;
                if(c == '.')
                {
                    textBuffer += "<br>";
                }

                bookText.text = textBuffer;
                yield return new WaitForSeconds(typeInterval);
            }

            yield return new WaitForSeconds(readingDuration);
            textBuffer = null;
            bookText.text = "";
        }

        animator.SetTrigger("Close");
        interactionManager.ChangeLevelIndex(gameObject.name);
    }
}
