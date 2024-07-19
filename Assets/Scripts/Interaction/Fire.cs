using System.Collections;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public ParticleSystem fireVFX;
    public Material burntMaterial;
    private bool isLighted = false;

    private void Start()
    {
        fireVFX.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<LightFire>(out LightFire fire))
            if (fire.isFired && !isLighted)
            {
                StartCoroutine(Burn(fire));
                isLighted = true;
            }
    }

    IEnumerator Burn(LightFire fire)
    {
        fireVFX.Play();
        
        yield return new WaitForSeconds(4f);
        fireVFX.Stop();
        MeshRenderer renderer = GetComponentInParent<MeshRenderer>();
        renderer.material = burntMaterial;

        fire.ChangeLevelIndex();
    }
}