using UnityEngine;

public class LightFire : MonoBehaviour
{
    public GameObject fireVFX;
    public Transform candleOffset;
    public InteractionManager interactionManager;

    private bool isLighted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Fire>(out Fire fire))
            if (fire.isFired && !isLighted)
            {
                Instantiate(fireVFX, candleOffset);
                interactionManager.ChangeLevelIndex(other.gameObject.transform.parent.name);
                isLighted = true;
            }      
    }

}
