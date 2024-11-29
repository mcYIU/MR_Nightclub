using UnityEngine;

public class Rose : MonoBehaviour
{
    [SerializeField] private Rose_Pluck[] rosePieces;
    [SerializeField] private Interactable interactable;

    private int availablePluck;
    private MeshRenderer mesh;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        availablePluck = rosePieces.Length; 
    }

    public void Pluck()
    {
        availablePluck--;

        if (availablePluck == 0)
        {
            if (mesh != null) mesh.enabled = false;

            interactable.IncreaseInteractionLevel();
        }
    }
}
