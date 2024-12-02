using Oculus.Interaction;
using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        Grab,
        Poke,
        None
    }

    public InteractionManager interactionManager;
    [SerializeField] private InteractableType type;
    [SerializeField] private Canvas[] interactionUI;
    [HideInInspector] public bool isInteractionEnabled;

    [SerializeField] private Grabbable[] grabbables;
    [SerializeField] private PokeInteractable[] pokeInteractables;

    private int interactionLevel;

    public void SetInteraction(bool isEnabled)
    {
        switch (type)
        {
            case InteractableType.None:
                break;
            case InteractableType.Grab:
                foreach (var grab in grabbables) grab.enabled = isEnabled;
                break;
            case InteractableType.Poke:
                foreach (var poke in pokeInteractables) poke.enabled = isEnabled;
                break;
        }

        SetUI(isEnabled);

        isInteractionEnabled = isEnabled;
    }

    public void SetInteractionLevel(int level)
    {
        interactionLevel = level;
    }

    public void SetUI(bool isActive)
    {
        foreach (Canvas c in interactionUI)
        {
            if (c != null) c.enabled = isActive;
        }
    }

    public void IncreaseInteractionLevel()
    {
        if (interactionManager.LevelIndex != interactionLevel)
        {
            interactionManager.ChangeLevelIndex(interactionLevel);
            SetUI(false);
        }
    }

    public void PlaySFX(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    SerializedProperty manager;
    SerializedProperty ui;
    SerializedProperty typeProp;
    SerializedProperty grabbablesProp;
    SerializedProperty pokeInteractablesProp;

    void OnEnable()
    {
        manager = serializedObject.FindProperty("interactionManager");
        ui = serializedObject.FindProperty("interactionUI");
        typeProp = serializedObject.FindProperty("type");
        grabbablesProp = serializedObject.FindProperty("grabbables");
        pokeInteractablesProp = serializedObject.FindProperty("pokeInteractables");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(manager);
        EditorGUILayout.PropertyField(ui);
        EditorGUILayout.PropertyField(typeProp);

        Interactable.InteractableType currentType = (Interactable.InteractableType)typeProp.enumValueIndex;

        switch (currentType)
        {
            case Interactable.InteractableType.Grab:
                EditorGUILayout.PropertyField(grabbablesProp, true);
                pokeInteractablesProp.isExpanded = false;
                break;
            case Interactable.InteractableType.Poke:
                EditorGUILayout.PropertyField(pokeInteractablesProp, true);
                grabbablesProp.isExpanded = false;
                break;
            case Interactable.InteractableType.None:
                grabbablesProp.isExpanded = false;
                pokeInteractablesProp.isExpanded = false;
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}