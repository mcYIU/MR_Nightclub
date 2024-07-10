using Obi;
using System.Collections;
using UnityEngine;

public class WhiskeyPour : MonoBehaviour
{
    public GameObject attachPoint;
    public GameObject obiFluid;
    public GameObject whiskeyInGlass;
    public float dissolveDuration = 4f;
    public InteractionManager interactionManager;

    private Material whiskey_Material;
    private Rigidbody bottleCap_rb;
    private Transform bottle;
    private Quaternion bottle_InitialRotation;
    private bool isOpened = false;

    void Start()
    {
        bottleCap_rb = GetComponent<Rigidbody>();
        obiFluid.SetActive(false);

        Renderer renderer = whiskeyInGlass.GetComponent<Renderer>();
        whiskey_Material = renderer.material;
        Color whiskey_Color = whiskey_Material.color;
        Debug.Log(whiskey_Color.ToString());
        whiskey_Color.a = 0f;

        bottle = transform.parent;
        bottle_InitialRotation = bottle.rotation;
    }

    private void Update()
    {
        if (!isOpened && Vector3.Distance(transform.position, attachPoint.transform.position) > 0.001f)
        {
            isOpened = true;
        }

        if (isOpened)
        {
            float angleDifference = Quaternion.Angle(bottle_InitialRotation, bottle.transform.rotation);
            if (angleDifference > 90f)
            {
                StartCoroutine(Pouring());
            }
            else
            {
                obiFluid.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isOpened)
        {
            bottleCap_rb.isKinematic = false;
            bottleCap_rb.useGravity = true;
            transform.SetParent(null);
        }
    }

    IEnumerator Pouring()
    {
        obiFluid.SetActive(true);
        yield return new WaitForSeconds(10f);

        interactionManager.ChangeLevelIndex(gameObject.name);
        StartCoroutine(WhiskeyAppearing());
        Drying();
    }

    void Drying()
    {
        obiFluid.TryGetComponent<ObiParticleRenderer>(out ObiParticleRenderer renderer);
        float currentAlpha = renderer.particleColor.a;
        currentAlpha -= Time.deltaTime / dissolveDuration;
        currentAlpha = Mathf.Clamp01(currentAlpha);
        renderer.particleColor.a = currentAlpha;
    }

    IEnumerator WhiskeyAppearing()
    {
        float elapsedTime = 0f;
        Color whiskey_Color = whiskey_Material.color;
        float startAlpha = whiskey_Material.color.a;

        while (elapsedTime < dissolveDuration)
        {
            float normalizedTime = elapsedTime / dissolveDuration;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, normalizedTime);

            whiskey_Color.a = newAlpha;

            yield return null;
            elapsedTime += Time.deltaTime;
        }
        
        whiskey_Color.a = 1f;
    }
}
