using UnityEngine;
using System.Collections;

public class TextAnimator : MonoBehaviour
{
    public ParticleSystem textEffect;
    public Transform centerPoint;
    public float minPosYOffset;
    public float maxPosYOffset;
    public float minRadius;
    public float maxRadius;
    public float minRotationSpeed;
    public float maxRotationSpeed;
    public float minFloatSpeed;
    public float maxFloatSpeed;

    private float radius;
    private float rotationSpeed;
    private float floatSpeed;
    private float floatHeight;
    private float angle;

    private void Start()
    {
        if (centerPoint == null) centerPoint = FindObjectOfType<Camera>().transform;
        if (textEffect == null) textEffect = GetComponentInChildren<ParticleSystem>();

        floatHeight = Random.Range(minPosYOffset, maxPosYOffset);
        radius = Random.Range(minRadius, maxRadius);
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        floatSpeed = Random.Range(minFloatSpeed, maxFloatSpeed);

        StartCoroutine(MoveInOrbit());
    }

    IEnumerator MoveInOrbit()
    {
        while (true)
        {
            // Circular motion around the center point
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            transform.position = centerPoint.position + offset;

            // Update angle for circular motion
            angle += rotationSpeed * Time.deltaTime / 100.0f;

            // Floating motion on the y-axis
            float newY = centerPoint.position.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }
    }
}