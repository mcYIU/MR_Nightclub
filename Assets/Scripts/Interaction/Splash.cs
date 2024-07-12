using UnityEngine;

public class Splash : MonoBehaviour
{
    public ParticleSystem splashParticle;

    private void OnCollisionEnter(Collision collision)
    {
        splashParticle.Play();
    }
}
