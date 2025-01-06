using UnityEngine;

public class Match_Fire : MonoBehaviour
{
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject matchBox;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private AudioClip SFX;
    [SerializeField] private Interactable interactable;

    [HideInInspector] public GameObject fireInstance;
    [HideInInspector] public bool isPicked = false;

    private void Update()
    {
        if (interactable.isInteractionEnabled)
        {
            if (CheckCollision(firePoint, matchBox) && isPicked)
            {
                if (fireInstance == null)
                {
                    SoundEffectManager.PlaySFXOnce(SFX);
                    fireInstance = Instantiate(firePrefab, firePoint.transform.position, Quaternion.identity);
                }
            }
        }

        if (fireInstance != null)
        {
            fireInstance.transform.position = firePoint.transform.position;
        }
    }

    private bool CheckCollision(GameObject obj1, GameObject obj2)
    {
        Collider collider1 = obj1.GetComponent<Collider>();
        Collider collider2 = obj2.GetComponent<Collider>();

        if (collider1 != null && collider2 != null)
        {
            return collider1.bounds.Intersects(collider2.bounds);
        }

        return false;
    }

    public void SetPickUp()
    {
        if (!isPicked) isPicked = true;
        else isPicked = false;
    }
}
