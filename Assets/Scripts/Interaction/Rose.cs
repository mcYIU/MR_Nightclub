using UnityEngine;

public class Rose : MonoBehaviour
{
    public InteractionManager interactionManager;
    public  int maxInteractionIndex = 2;

    private int interactionIndex = 0;
    
    public void AddIndex()
    {
        interactionIndex++;
        if (interactionIndex == maxInteractionIndex)
        {
            interactionManager.ChangeLevelIndex(gameObject.name);
        }
    }
}
