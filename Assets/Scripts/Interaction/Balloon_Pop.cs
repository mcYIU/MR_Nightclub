using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{
    public GameObject explosionVFX;
    public InteractionManager interactionManager;
    //int completedLevel = 0;
    //int maxLevel = 2;
    string objectName;

    private void Start()
    {
        objectName = gameObject.name;
    }

    public void Poke()
    {
        //completedLevel++;

        Destroy(gameObject);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        //if (completedLevel == maxLevel)
            interactionManager.ChangeLevelIndex(objectName);
    }
}
