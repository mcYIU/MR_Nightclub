using UnityEngine;

public class Whiskey_Pour : MonoBehaviour
{
    //public GameObject attachPoint;
    public ParticleSystem fluid;
    public float pouringAngle;
    public InteractionManager interactionManager;

    //public AudioSource AS_OpenCap;
    //public AudioSource AS_DropCap;

    //private Rigidbody rb;
    //private Transform bottle;
    private Quaternion bottle_InitialRotation;

    //private bool isOpened = false;
    private bool isPouring = false;
    private bool isBottleHeld = false;


    void Start()
    {
        //rb = GetComponent<Rigidbody>();

        //bottle = transform.parent;
        bottle_InitialRotation = transform.localRotation;
    }

    private void Update()
    {
        if (isBottleHeld)
        {
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
