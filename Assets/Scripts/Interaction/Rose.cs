using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose : MonoBehaviour
{
    public InteractionManager interactionManager;
    public int maxInteractionIndex = 2;
    int interactionIndex = 0;

    private void OnEnable() => Pluck.OnBoolChanged += InteractionIndexChanged;

    private void OnDisable() => Pluck.OnBoolChanged -= InteractionIndexChanged;

    private void InteractionIndexChanged(bool pluck)
    {
        if(pluck)
        {
            interactionIndex++;
        }
        if (interactionIndex == maxInteractionIndex)
        {
            interactionManager.ChangeLevelIndex(gameObject.name);
        }
    }
}
