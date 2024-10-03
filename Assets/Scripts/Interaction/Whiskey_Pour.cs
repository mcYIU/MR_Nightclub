using UnityEngine;

public class Whiskey_Pour : MonoBehaviour
{
    public ParticleSystem fluid;
    public float pouringAngle;
    public InteractionManager interactionManager;

    private Quaternion bottle_InitialRotation;
    private bool isPouring = false;
    private bool isBottleHeld = false;

    private void Update()
    {
        if (isBottleHeld)
        {
            bottle_InitialRotation = transform.localRotation;

            bool pourCheck = CalculatePourAngle() > pouringAngle;
            if (isPouring != pourCheck)
            {
                isPouring = pourCheck;
                if (isPouring)
                {
                    fluid.Play();
                }
                else
                {
                    fluid.Stop();
                }
            }
        }
        else
        {
            fluid.Stop();
        }
    }

    public void HoldBottle()
    {
        isBottleHeld = true;
    }

    public void ReleaseBottle()
    {
        isBottleHeld = false;
    }

    private float CalculatePourAngle()
    {
        return Quaternion.Angle(bottle_InitialRotation, transform.localRotation);
    }

    public void ChangeLevelIndex()
    {
        if(interactionManager != null)
            interactionManager.ChangeLevelIndex(gameObject.name);
    }
}

//backup
/*private void OnCollisionEnter(Collision collision)
{
if (collision.collider.gameObject.CompareTag("Environment"))
{
    AS_DropCap.Play();
}
}

public void CapPhysics()
{
if (!isOpened)
{
    isOpened = true;

    AS_OpenCap.Play();

    rb.isKinematic = false;
    rb.useGravity = true;
    transform.SetParent(null);
}
}*/
