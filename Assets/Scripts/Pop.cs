using UnityEngine;

public class Pop : MonoBehaviour
{ 
    public GameObject explosionVFX;

    public void Poke()
    {
        Destroy(gameObject);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
    }
}
