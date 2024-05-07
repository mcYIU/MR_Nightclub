using System.Transactions;
using UnityEngine;

public class DialogueTrigger_D3 : MonoBehaviour
{
    public Dialogue dialogue;
    DialogueManager_D3 dialogueManager_d3;

    bool isTrigger = false;

    public GameObject gameobj;


    private void Start()
    {
        dialogueManager_d3 = FindAnyObjectByType<DialogueManager_D3>();  
        if(gameobj != null )
            gameobj.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTrigger)
        {
            if (other.gameObject.tag != "Match" || other.gameObject.name != "default")
            {
                print(other.gameObject.name);
                dialogueManager_d3.StartDialogue(dialogue);
                isTrigger = true;
                if(gameobj != null )
                    gameobj.SetActive(true);
            }
        }
    }
}
