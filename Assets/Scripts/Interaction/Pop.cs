using System.Collections;
using UnityEngine;

public class Pop : MonoBehaviour
{ 
    public GameObject explosionVFX;
    public InteractionManager interactionManager;

    public void Poke()
    {
        Destroy(gameObject);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        StartCoroutine(ChangeLevelIndex());
    }

    IEnumerator ChangeLevelIndex()
    {
        yield return new WaitForSeconds(1f);
        interactionManager.ChangeLevelIndex(gameObject.name);
    }
}
