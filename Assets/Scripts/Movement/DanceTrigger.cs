using UnityEngine;

public class DanceTrigger : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer target;
    [SerializeField] private Color completionColor;
    [SerializeField] DanceQuestManager manager;
    private Material material;

    private void Start()
    {
        material = target.material;
    }

    public void NextPose()
    {
        material.color = completionColor;

        if (manager) manager.CheckPoseCompletion();
    }
}
