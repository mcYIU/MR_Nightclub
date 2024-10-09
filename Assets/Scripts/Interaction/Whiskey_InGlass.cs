using UnityEngine;

public class Whiskey_InGlass : MonoBehaviour
{
    public float fillSpeed;
    public Whiskey_Pour whiskeyPour;
    public AudioSource audioSource;
    public Canvas interactionUI;

    private Renderer sphereRenderer;
    private float maxHeight;
    private float currentHeight = 0f;
    private bool isPoured = false;

    private void Start()
    {
        maxHeight = transform.localScale.y;
        sphereRenderer = GetComponent<Renderer>();
        sphereRenderer.enabled = false;

        interactionUI.enabled = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(!isPoured && currentHeight != maxHeight)
        {
            audioSource.Play();
            interactionUI.enabled = false;
            isPoured = true;
        }
    }

    private void Update()
    {
        if (isPoured)
        {         
            currentHeight += fillSpeed * Time.deltaTime;
            currentHeight = Mathf.Clamp(currentHeight, 0.0f, maxHeight);
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);

            sphereRenderer.enabled = true;

            if(currentHeight == maxHeight)
            {
                if(whiskeyPour != null)
                {
                    whiskeyPour.ChangeLevelIndex();
                }

                audioSource.Stop();
                isPoured = false;
            }
        }
    }

}