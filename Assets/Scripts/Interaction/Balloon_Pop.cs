using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{ 
    public GameObject explosionVFX;
    public InteractionManager interactionManager;
    string objectName;

    private void Start()
    {
        objectName = gameObject.name;
    }

    public void Poke()
    {
        Destroy(gameObject);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        interactionManager.ChangeLevelIndex(objectName);
    }
}
