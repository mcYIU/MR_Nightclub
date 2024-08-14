using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Match_Fracture : MonoBehaviour
{
    public GameObject fracturedObjectPrefab;
    public HandGrabInteractor leftInteractor;
    public HandGrabInteractor rightInteractor;

    HandGrabInteractable interactable;

    private void Start()
    {
        interactable = GetComponent<HandGrabInteractable>();
    }

    private void Update()
    {
        if (leftInteractor.SelectedInteractable == interactable && rightInteractor.SelectedInteractable == interactable)
        {
            Instantiate(fracturedObjectPrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}