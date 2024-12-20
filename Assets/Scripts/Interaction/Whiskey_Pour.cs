using UnityEngine;

public class Whiskey_Pour : MonoBehaviour
{ 
    [SerializeField] private ParticleSystem fluidVisual;
    [SerializeField] private float pouringAngle;
    [SerializeField] private Interactable interactable;

    private Quaternion initialRotation;
    private bool isBottleHeld = false;
    private bool isPouring = false;

    private void Start()
    {
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        if (isBottleHeld && interactable.isInteractionEnabled)
        {
            bool pourCheck = CalculatePourAngle() > pouringAngle;
            if (isPouring != pourCheck)
            {
                isPouring = pourCheck;

                if (isPouring)
                {
                    fluidVisual.Play();
                }
                else
                {
                    fluidVisual.Stop();
                }
            }
        }
        else
        {
            fluidVisual.Stop();
        }
    }

    private float CalculatePourAngle()
    {
        return Quaternion.Angle(initialRotation, transform.localRotation);
    }

    public void ToggleBottle()
    {
        isBottleHeld = !isBottleHeld;
    }
}
