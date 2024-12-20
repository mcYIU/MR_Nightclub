using UnityEngine;

public class Rose_Pluck : MonoBehaviour
{
    [SerializeField] Transform detechPoint;
    [SerializeField] float detachDistance;
    [SerializeField] AudioClip SFX;
    [SerializeField] Rose rose;
    [SerializeField] Interactable interactable;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Pluck()
    {
        interactable.SetUI(false);

        if (IsPlucked() && interactable.isInteractionEnabled)
        {
            PickOutFromParent();
        }
    }

    private bool IsPlucked()
    {
        return Vector3.Distance(transform.position, detechPoint.position) < detachDistance;
    }

    private void PickOutFromParent()
    {
        SoundEffectManager.PlaySFXOnce(SFX);

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        rose.Pluck();
    }
}
