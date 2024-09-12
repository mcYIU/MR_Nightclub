using UnityEngine;

public class Rose : MonoBehaviour
{
    public MeshRenderer mesh;
    public InteractionManager interactionManager;
    public int maxInteractionIndex;

    private int interactionIndex = 0;

    public void AddIndex()
    {
        interactionIndex++;
        if (interactionIndex == maxInteractionIndex)
        {
            if (mesh != null) mesh.enabled = false;

            interactionManager.ChangeLevelIndex(gameObject.name);
        }
    }
}
