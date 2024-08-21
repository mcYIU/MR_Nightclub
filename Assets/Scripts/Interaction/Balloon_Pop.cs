using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{
    public ParticleSystem explosionVFX;
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
        //Instantiate(explosionVFX, transform.position, Quaternion.identity);
        //if (completedLevel == maxLevel)

        explosionVFX.Play();
        Destroy(gameObject);

        interactionManager.ChangeLevelIndex(objectName);
    }
}
