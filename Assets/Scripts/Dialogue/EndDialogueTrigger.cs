using UnityEngine;

public class EndDialogueTrigger : MonoBehaviour
{
    public Dialogue[] VO_Text;
    public AudioClip[] VO_Audio;
    public GameObject[] characters;

    private void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
    }
}
