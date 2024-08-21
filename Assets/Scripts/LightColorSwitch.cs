using UnityEngine;

public class LightColorSwitch : MonoBehaviour
{
    public Light spotlight;
    public float colorChangeSpeed = 1f;
    public float colorChangeInterval = 1f;

    private float t = 0f;
    private Color[] neonColors = { Color.red, Color.blue, Color.green, Color.magenta, Color.cyan, Color.yellow };
    private int currentIndex = 0;

    void Update()
    {
        t += Time.deltaTime * colorChangeSpeed;

        if (t >= colorChangeInterval)
        {
            t = 0f;
            currentIndex = (currentIndex + 1) % neonColors.Length;
        }

        Color startColor = neonColors[currentIndex];
        Color endColor = neonColors[(currentIndex + 1) % neonColors.Length];

        float lerpT = t / colorChangeInterval;
        Color lerpedColor = Color.Lerp(startColor, endColor, lerpT);

        spotlight.color = lerpedColor;
    }
}
