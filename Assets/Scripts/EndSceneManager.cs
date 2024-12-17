using System;
using UnityEngine;

public class EndSceneManager : MonoBehaviour
{
    [Serializable]
    private struct NPC
    {
        public GameObject gameObject;
        public DialogueTrigger dialogueTrigger;
    }

    [SerializeField] private NPC npc;
    private OVRPassthroughLayer layer;

    void Start()
    {
        layer = FindObjectOfType<OVRPassthroughLayer>();
        if (layer.textureOpacity == 0f) layer.textureOpacity = 1.0f;

        StartNPCDialogue();
    }

    private void StartNPCDialogue()
    {
        Dialogue _NPC_Dialogue = npc.dialogueTrigger.dialogues[0];
        Canvas _NPC_Canvas = npc.dialogueTrigger.UIElements.dialogueCanvas;
        npc.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger _NPC_Trigger);

        DialogueManager.StartDialogue(_NPC_Dialogue, _NPC_Canvas, _NPC_Trigger);
    }


}
