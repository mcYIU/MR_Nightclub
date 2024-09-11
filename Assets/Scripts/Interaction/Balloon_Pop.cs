using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{
    public ParticleSystem explosionVFX;
    public InteractionManager interactionManager;

    public void Poke()
    {
        explosionVFX.Play();
        Destroy(gameObject);

        if(interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
            interactionManager.ChangeLevelIndex(gameObject.name);
    }
}
