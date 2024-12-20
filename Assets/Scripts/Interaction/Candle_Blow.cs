using UnityEngine;

public class Candle_Blow : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer face;
    [SerializeField] private int blendshapeIndex;
    [SerializeField] private float triggerFloat;
    [SerializeField] private AudioClip SFX;
    [SerializeField] Interactable interactable;

    private void OnTriggerStay(Collider other)
    {
        if (interactable.isInteractionEnabled)
        {
            float weightIndex = face.GetBlendShapeWeight(blendshapeIndex);

            if (weightIndex > triggerFloat)
            {
                Blow();
            }
        }
    }

    private void Blow()
    {
        SoundEffectManager.PlaySFXOnce(SFX);

        interactable.IncreaseInteractionLevel();

        Destroy(gameObject);
    }
}
