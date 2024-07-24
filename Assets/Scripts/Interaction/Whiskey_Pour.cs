using UnityEngine;

public class Whiskey_Pour : MonoBehaviour
{
    public GameObject attachPoint;
    public ParticleSystem fluid;
    public float pouringAngle = 70f;
    public InteractionManager interactionManager;

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
        if (!isOpened && Vector3.Distance(transform.position, attachPoint.transform.position) > 0.001f)
        {
            isOpened = true;
        }

        if (isOpened)
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
    }

    private void FixedUpdate()
    {
        if (isOpened)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            transform.SetParent(null);
        }
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
