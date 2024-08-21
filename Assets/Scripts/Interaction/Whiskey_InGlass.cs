using UnityEngine;

public class Whiskey_InGlass : MonoBehaviour
{
    public float fillSpeed = 0.1f;
    public Whiskey_Pour whiskeyPour;
    public AudioSource AS_Pour;

    private Renderer sphereRenderer;
    private float maxHeight;
    private float currentHeight = 0f;
    private bool isPoured = false;

    private void Start()
    {
        maxHeight = transform.localScale.y;
        sphereRenderer = GetComponent<Renderer>();
        sphereRenderer.enabled = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(!isPoured && currentHeight != maxHeight)
        {
            AS_Pour.Play();
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

                AS_Pour.Stop();
                isPoured = false;
            }
        }
    }

}