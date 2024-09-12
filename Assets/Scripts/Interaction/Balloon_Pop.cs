using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{
    public InteractionManager interactionManager;

    public void Poke()
    {
        Destroy(gameObject);

        if(interactionManager != null && interactionManager.LevelIndex < interactionManager.ineteractionLayerCount)
            interactionManager.ChangeLevelIndex(gameObject.name);
    }
}
