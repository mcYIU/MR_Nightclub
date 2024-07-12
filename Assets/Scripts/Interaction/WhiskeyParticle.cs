using UnityEngine;

public class WhiskeyParticle : MonoBehaviour
{
    WhiskeyInGlass whiskeyInGlass;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("yes");
        other.gameObject.TryGetComponent<WhiskeyInGlass>(out WhiskeyInGlass whiskey);
        whiskeyInGlass = whiskey;
        whiskeyInGlass.isPoured = true;
    }

    private void OnParticleSystemStopped()
    {
        whiskeyInGlass.isPoured = false;
    }
}
