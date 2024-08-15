using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Candle_Blow : MonoBehaviour
{
    public SkinnedMeshRenderer face;
    public int blendshapeIndex;
    public float triggerFloat;
    public AudioSource audioSource;
    public InteractionManager interactionManager;

    private void OnTriggerStay(Collider other)
    {
        if
        (transform.parent.TryGetComponent<HandGrabInteractable>(out HandGrabInteractable handGrabInteractable)
        && handGrabInteractable.enabled)
        {
            float weightIndex = face.GetBlendShapeWeight(blendshapeIndex);
            if (weightIndex > triggerFloat)
            {
                audioSource.Play();
                interactionManager.ChangeLevelIndex(transform.parent.name);
                gameObject.SetActive(false);
            }
        }
    }
}
