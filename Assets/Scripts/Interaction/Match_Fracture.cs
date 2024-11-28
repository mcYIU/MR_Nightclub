using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Match_Fracture : MonoBehaviour
{
    [SerializeField] private GameObject fracturedMatch;
    [SerializeField] private HandGrabInteractable[] interactables;

    public void Fracture()
    {
        int grabCount = 0;

        for (int i = 0; i < interactables.Length; i++)
            if (interactables[i].Interactors.Count > 0)
                grabCount++;

        if (grabCount == interactables.Length)
        {
            Instantiate(fracturedMatch, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}