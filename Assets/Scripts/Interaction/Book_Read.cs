using System.Collections;
using TMPro;
using UnityEngine;

public class Book_Read : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string[] sentences;
    [SerializeField] private TextMeshProUGUI bookText;
    [SerializeField] private float animationDurationOffset;
    [SerializeField] private float typeInterval;
    [SerializeField] private float readingDuration;
    [SerializeField] private AudioClip[] SFX;
    [SerializeField] private TextMeshProUGUI noticeText;
    [SerializeField] private Interactable interactable;

    private bool isRead = false;

    private void Start()
    {
        bookText.text = string.Empty;
    }

    public void ReadText()
    {
        if (interactable.isInteractionEnabled && !isRead)
        {
            isRead = true;

            noticeText.text = string.Empty;
            interactable.SetUI(false);

            StartCoroutine(Type());
        }
    }

    private IEnumerator Type()
    {
        animator.SetTrigger("Open");

        yield return new WaitForSeconds(animationDurationOffset);

        for (int i = 0; i < sentences.Length; i++)
        {
            int _i = Random.Range(0, SFX.Length);
            SoundEffectManager.PlaySFXOnce(SFX[_i]);

            string textBuffer = null;
            foreach (char c in sentences[i])
            {
                textBuffer += c;
                if (c == '.')
                {
                    textBuffer += "<br>";
                }

                bookText.text = textBuffer;
                yield return new WaitForSeconds(typeInterval);
            }

            yield return new WaitForSeconds(readingDuration);

            textBuffer = string.Empty;
            bookText.text = string.Empty;
        }

        animator.SetTrigger("Close");

        interactable.IncreaseInteractionLevel();
    }
}
