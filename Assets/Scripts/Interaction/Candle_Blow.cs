using UnityEngine;

public class Candle_Blow : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer face;
    [SerializeField] private int[] blendshapeIndex;
    [SerializeField] private float triggerFloat;
    [SerializeField] private AudioClip SFX;
    [SerializeField] Interactable interactable;

    private void OnTriggerStay(Collider other)
    {
        if (interactable.isInteractionEnabled && other.GetComponent<SkinnedMeshRenderer>() == face)
        {
            if (CheckBlendshapeWeight())
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

    private float GetBlendshapeWeight(int index)
    {
        return face.GetBlendShapeWeight(index);
    }

    private bool CheckBlendshapeWeight()
    {
        int triggeredIndex = 0;

        foreach (var _index in blendshapeIndex)
        {
            if (GetBlendshapeWeight(_index) > triggerFloat)
                triggeredIndex++;
        }

        return triggeredIndex > 0;
    }
}
