using UnityEngine;

public class Pop : MonoBehaviour
{ 
    public GameObject explosionVFX;
    public InteractionManager interactionManager;

    public void Poke()
    {
        Destroy(gameObject);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        interactionManager.ChangeLevelIndex(gameObject.name);
    }
}
