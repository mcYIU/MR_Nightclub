using UnityEngine;

public class Rose_Pluck : MonoBehaviour
{
    [SerializeField] Transform detechPoint;
    [SerializeField] float detachDistance;
    [SerializeField] AudioClip SFX;
    [SerializeField] Interactable interactable;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody> ();
    }

    public void Pluck()
    {
        if (Vector3.Distance(transform.position, detechPoint.position) < detachDistance)
        {
            PickOutFromParent();
        }
    }

    private void PickOutFromParent()
    {
        SoundEffectManager.PlaySFXOnce(SFX);

        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
