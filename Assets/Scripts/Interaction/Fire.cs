using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject fireVFX;
    public GameObject firePoint;
    private bool isLighted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<LightFire>(out LightFire fire))
            if (fire.isFired && !isLighted)
            {
                Instantiate(fireVFX, firePoint.transform);
                isLighted = true;
                fire.ChangeLevelIndex();
            }
    }
}