using System.Collections;
using UnityEngine;

public class Champagne_Splash : MonoBehaviour
{
    [SerializeField] private ParticleSystem pouringVisual;
    [SerializeField] private GameObject capPrefab;
    [SerializeField] private float prefabLifeTime;
    [SerializeField] private AudioClip SFX; 
    [SerializeField] private Interactable interactable;

    private GameObject capInstance;

    public void Pouring()
    {
        if (interactable.isInteractionEnabled)
        {
            pouringVisual.Play();
            SoundEffectManager.PlaySFXOnce(SFX);

            interactable.SetInteraction(false);
            interactable.IncreaseInteractionLevel();

            StartCoroutine(ReleaseCap());
        }
    }

    private IEnumerator ReleaseCap()
    {
        MeshRenderer _mesh = GetComponent<MeshRenderer>();
        _mesh.enabled = false;

        capInstance = Instantiate(capPrefab);

        yield return new WaitForSeconds(prefabLifeTime);

        Destroy(capInstance);
    }
}
