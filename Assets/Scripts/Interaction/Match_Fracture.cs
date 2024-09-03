using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Match_Fracture : MonoBehaviour
{
    public GameObject fracturedObjectPrefab;
    public HandGrabInteractable[] interactables;
    //public HandGrabInteractor leftInteractor;
    //public HandGrabInteractor rightInteractor;

    //HandGrabInteractable interactable;

    private void Start()
    {
        //interactable = GetComponent<HandGrabInteractable>();
    }

    /*private void Update()
    {
        if (leftInteractor != null && rightInteractor != null)
            if (leftInteractor.SelectedInteractable == interactable && rightInteractor.SelectedInteractable == interactable)
            {
                CreateFracturedObject();
            }
    }*/

    private void CreateFracturedObject()
    {
        Instantiate(fracturedObjectPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Fracture()
    {
        int grabCount = 0;

        for (int i = 0; i < interactables.Length; i++)
            if (interactables[i].Interactors.Count > 0)
                grabCount++;

        if (grabCount == interactables.Length)
        {
            Instantiate(fracturedObjectPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}