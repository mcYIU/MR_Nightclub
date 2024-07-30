using UnityEngine;

public class Whiskey_Pour : MonoBehaviour
{
    public GameObject attachPoint;
    public ParticleSystem fluid;
    public float pouringAngle = 70f;
    public InteractionManager interactionManager;

    public AudioSource AS_Pour;
    public AudioSource AS_OpenCap;
    public AudioSource AS_DropCap;

    private Rigidbody rb;
    private Transform bottle;
    private Quaternion bottle_InitialRotation;

    private bool isOpened = false;
    private bool isPouring = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        bottle = transform.parent;
        bottle_InitialRotation = bottle.rotation;
    }

    private void Update()
    {
        if (isOpened)
        {
            bool pourCheck = CalculatePourAngle() > pouringAngle;
            if (isPouring != pourCheck)
            {
                isPouring = pourCheck;
                if (isPouring)
                {
                    fluid.Play();
                    AS_Pour.Play();
                }
                else
                {
                    fluid.Stop();
                    AS_Pour.Stop();
                }
            }
        }     
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Environment"))
        {
            AS_DropCap.Play();
        }
    }

    public void CapPhysics()
    {
        isOpened = true ;

        AS_OpenCap.Play();

        rb.isKinematic = false;
        rb.useGravity = true;
        transform.SetParent(null);
    }

    private float CalculatePourAngle()
    {
        return Quaternion.Angle(bottle_InitialRotation, bottle.transform.rotation);
    }

    public void ChangeLevelIndex()
    {
        interactionManager.ChangeLevelIndex(gameObject.name);
    }
}
