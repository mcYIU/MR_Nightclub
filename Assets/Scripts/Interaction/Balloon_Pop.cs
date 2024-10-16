using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{
    public InteractionManager interactionManager;

    public void Poke()
    {
        Destroy(gameObject);

        if(interactionManager != null && interactionManager.LevelIndex == 0)
            interactionManager.ChangeLevelIndex(gameObject.name);
    }
}
