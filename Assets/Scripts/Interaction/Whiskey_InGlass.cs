using System.Collections;
using UnityEngine;

public class Whiskey_InGlass : MonoBehaviour
{
    [SerializeField] private float fillSpeed;
    [SerializeField] private AudioClip SFX;
    [SerializeField] private Interactable interactable;

    private Renderer sphereRenderer;
    private float maxHeight;
    private float currentHeight = 0f;

    private bool isWhiskeyPouring = false;

    private void Start()
    {
        maxHeight = transform.localScale.y;
        sphereRenderer = GetComponent<Renderer>();
        sphereRenderer.enabled = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Whiskey") && !isWhiskeyPouring && currentHeight != maxHeight)
        {
            StartPouring();
        }
    }

    private void StartPouring()
    {
        isWhiskeyPouring = true;
        interactable.SetUI(!isWhiskeyPouring);

        StartCoroutine(PourWhiskey());
    }

    private void CompletePouring()
    {
        isWhiskeyPouring = false;
        StopAllCoroutines();

        interactable.IncreaseInteractionLevel();
    }

    private IEnumerator PourWhiskey()
    {
        SoundEffectManager.PlaySFXLoop(SFX);

        while (currentHeight < maxHeight)
        {
            currentHeight += fillSpeed * Time.deltaTime;
            currentHeight = Mathf.Clamp(currentHeight, 0.0f, maxHeight);
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);

            sphereRenderer.enabled = true;

            yield return null;
        }

        SoundEffectManager.StopSFXLoop();
        CompletePouring();
    }
}