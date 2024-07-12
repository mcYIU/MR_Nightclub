using UnityEngine;

public class WhiskeyInGlass : MonoBehaviour
{
    public float visibilityTime;
    public WhiskeyPour whiskeyPour;
    public bool isPoured = false;

    private Renderer sphereRenderer;
    private float timer = 0f;

    private void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
        sphereRenderer.enabled = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("yes");
        isPoured = true;
    }

    private void OnParticleSystemStopped()
    {
        Debug.Log("no");
        isPoured = false;
    }

    private void Update()
    {
        if (isPoured)
        {
            // Update the timer
            timer += Time.deltaTime;
            sphereRenderer.enabled = true;

            // Calculate the visibility percentage based on the timer and total time
            float visibilityPercentage = Mathf.Clamp01(timer / visibilityTime);

            // Set the scale of the sphere based on the visibility percentage
            Vector3 scale = transform.localScale;
            scale.y = visibilityPercentage;
            transform.localScale = scale;

            // Set the transparency of the sphere based on the visibility percentage
            //Color sphereColor = sphereRenderer.material.color;
            //sphereColor.a = visibilityPercentage;
            //sphereRenderer.material.color = sphereColor;

            whiskeyPour.ChangeLevelIndex();
        }
    }

}