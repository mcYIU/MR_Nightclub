using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Pluck : MonoBehaviour
{
    public delegate void BoolChanged(bool newValue);
    public static event BoolChanged OnBoolChanged;

    private bool isPlucked = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<HandGrabInteractor>(out HandGrabInteractor interactor))
            if(interactor.Interactable == gameObject)
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                transform.SetParent(null, false);
            }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<HandGrabInteractor>())
        {
            isPlucked = true;
            OnBoolChanged?.Invoke(isPlucked);
        }
    }
}
