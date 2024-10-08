using UnityEngine;

public class FaceTo : MonoBehaviour
{
    public Transform playerPos;

    private void Start()
    {
        if(playerPos == null)
            playerPos = FindObjectOfType<Camera>().transform;
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            Vector3 directionToPlayer = playerPos.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            Quaternion reversedRotation = targetRotation * Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = reversedRotation;
        }
    }
}
