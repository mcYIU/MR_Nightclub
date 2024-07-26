using UnityEngine;

public class Whiskey_InGlass : MonoBehaviour
{
    public float fillSpeed = 0.1f;
    public Whiskey_Pour whiskeyPour;
    [HideInInspector] public bool isPoured = false;

    private Renderer sphereRenderer;
    private float maxHeight;
    private float currentHeight = 0f;

    private void Start()
    {
        maxHeight = transform.localScale.y;
        sphereRenderer = GetComponent<Renderer>();
        sphereRenderer.enabled = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(!isPoured)
        {
            isPoured = true;
        }
    }

    private void OnParticleSystemStopped()
    {
        isPoured = false;
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
                whiskeyPour.ChangeLevelIndex();
            }
        }
    }

}