using System.Collections;
using UnityEngine;

public class Rose : MonoBehaviour
{
    public InteractionManager interactionManager;
    int interactionIndex = 0;
    readonly int maxInteractionIndex = 2;
    
    public void AddIndex()
    {
        interactionIndex++;
        if (interactionIndex == maxInteractionIndex)
        {
            StartCoroutine(ChangeLevelIndex());
        }
    }

    IEnumerator ChangeLevelIndex()
    {
        yield return new WaitForSeconds(1f);
        interactionManager.ChangeLevelIndex(gameObject.name);
    }
}
