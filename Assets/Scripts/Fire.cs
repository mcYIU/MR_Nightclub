using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject matchBox;
    public GameObject firePrefab;

    private GameObject fireInstance;
    private bool isFired = false;

    private void Update()
    {
        if (CheckCollision(gameObject, matchBox))
        {
            if (!isFired)
            {
                fireInstance = Instantiate(firePrefab, transform.position, Quaternion.identity);
                isFired = true;
            }
        }

        if (fireInstance != null)
        {
            fireInstance.transform.position = transform.position;
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
}