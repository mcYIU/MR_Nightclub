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
    [SerializeField] private float dialogueDelay;
    private OVRPassthroughLayer layer;

    void Start()
    {
        layer = FindObjectOfType<OVRPassthroughLayer>();
        if (layer.textureOpacity == 0f) layer.textureOpacity = 1.0f;

        Invoke(nameof(StartNPCDialogue), dialogueDelay);
    }

    private void StartNPCDialogue()
    {
        npc.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger _NPC_Trigger);
        _NPC_Trigger.StartDialogue();
    }


}
