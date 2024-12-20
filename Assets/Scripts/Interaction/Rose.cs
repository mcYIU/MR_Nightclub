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

        SetRosePiece(false);
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

    public void SetRoseInteraction(bool isActive)
    {
        if (interactable.isInteractionEnabled)
        {
            SetRosePiece(isActive);
        }
    }

    private void SetRosePiece(bool isActive)
    {
        foreach (var _piece in rosePieces)
        {
            _piece.gameObject.SetActive(isActive);
        }
    }
}
