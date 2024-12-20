using UnityEngine;

public class Dice_Throw : MonoBehaviour
{
    public static bool isGrounded = true;
    [SerializeField] private Interactable interactable;
    [SerializeField] private AudioClip[] SFX;
    [SerializeField] private float throwForce;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Environment") && !isGrounded)
        {
            ThrowOnTable();
        }
    }

    private void ThrowOnTable()
    {
        int soundIndex = Random.Range(0, SFX.Length);
        SoundEffectManager.PlaySFXOnce(SFX[soundIndex]);

        interactable.IncreaseInteractionLevel();
    }

    public void GrabAllDices(Transform parentDice)
    {
        if (interactable.isInteractionEnabled)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;

            transform.SetPositionAndRotation(parentDice.position, parentDice.rotation);
            transform.SetParent(parentDice.transform);
            transform.localScale = Vector3.one;

            isGrounded = false;

            SetUI(isGrounded);
        }
    }

    public void ReleaseAllDices()
    {
        if (interactable.isInteractionEnabled)
        {
            transform.SetParent(null);
            transform.localScale = Vector3.one;

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }
    }

    private void SetUI(bool isGrabbed)
    {
        interactable.SetUI(isGrabbed);
    }
}
