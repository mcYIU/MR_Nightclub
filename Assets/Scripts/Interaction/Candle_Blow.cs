using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Candle_Blow : MonoBehaviour
{
    public SkinnedMeshRenderer face;
    public int blendshapeIndex;
    public float triggerFloat;
    public InteractionManager interactionManager;

    private void OnTriggerStay(Collider other)
    {
        transform.parent.TryGetComponent<HandGrabInteractable> (out HandGrabInteractable handGrabInteractable);
        if (handGrabInteractable.enabled)
        {
            float weightIndex = face.GetBlendShapeWeight(blendshapeIndex);
            if (weightIndex > triggerFloat)
            {
                interactionManager.ChangeLevelIndex(transform.parent.name);
                gameObject.SetActive(false);
            }
        }
    }
}
