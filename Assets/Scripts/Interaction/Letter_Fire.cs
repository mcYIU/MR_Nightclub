using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;

public class Letter_Fire : MonoBehaviour
{
    public ParticleSystem fireVFX;
    public float burningDuration = 5.0f;
    public string alphaClipPropertyName = "_Cutoff";
    public float targetAlphaThreshold = 0.8f;

    private Renderer objectRenderer;
    private Rigidbody rb;
    private HandGrabInteractable interactable;
    private float initialAlphaThreshold;
    private Color initialColor;
    private Color targetColor = Color.black;

    private bool isLighted = false;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        interactable = GetComponentInParent<HandGrabInteractable>();

        objectRenderer = GetComponentInParent<Renderer>();
        if(objectRenderer != null)
        {
            initialColor = objectRenderer.material.color;
            initialAlphaThreshold = objectRenderer.material.GetFloat(alphaClipPropertyName);
        }  

        fireVFX.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Match_Fire>(out Match_Fire fire))
            if (fire.isFired && !isLighted)
            {
                StartCoroutine(Burn(fire));
                isLighted = true;
            }
    }

    private IEnumerator FadeToAsh()
    {
        float elapsedTime = 0f;

        while (elapsedTime < burningDuration)
        {
            float normalizedTime = elapsedTime / burningDuration;

            float currentAlphaThreshold = Mathf.Lerp(initialAlphaThreshold, targetAlphaThreshold, normalizedTime);
            objectRenderer.material.SetFloat(alphaClipPropertyName, currentAlphaThreshold);

            float currentAlpha = Mathf.Lerp(initialColor.a, targetColor.a, normalizedTime);
            Color newColor = new Color(targetColor.r, targetColor.g, targetColor.b, currentAlpha);
            objectRenderer.material.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectRenderer.material.color = targetColor;
        objectRenderer.material.SetFloat(alphaClipPropertyName, targetAlphaThreshold);
    }

    private IEnumerator Burn(Match_Fire fire)
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        interactable.enabled = false;

        fireVFX.Play();
        StartCoroutine(FadeToAsh());

        yield return new WaitForSeconds(burningDuration);
        fireVFX.Stop();

        fire.ChangeLevelIndex();
    }
}