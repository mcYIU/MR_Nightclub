using Oculus.Interaction;
using UnityEditor;
using UnityEngine;

[System.Serializable]
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

    [SerializeField]
    [ShowIfEnum("type", (int)InteractableType.Grab)]
    private Grabbable[] grabbables;

    [SerializeField]
    [ShowIfEnum("type", (int)InteractableType.Poke)]
    private PokeInteractable[] pokeInteractables;

    private int interactionLevel;

    public void SetInteraction(bool isEnabled)
    {
        foreach (var grab in  grabbables) grab.enabled = isEnabled;
        foreach (var poke in pokeInteractables) poke.enabled = isEnabled;

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

// Custom PropertyAttribute for showing/hiding fields based on enum value
public class ShowIfEnumAttribute : PropertyAttribute
{
    public string enumVariable;
    public int enumValue;

    public ShowIfEnumAttribute(string enumVariable, int enumValue)
    {
        this.enumVariable = enumVariable;
        this.enumValue = enumValue;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
public class ShowIfEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIfAttribute = (ShowIfEnumAttribute)attribute;
        SerializedProperty enumProperty = property.serializedObject.FindProperty(showIfAttribute.enumVariable);

        if (enumProperty != null && enumProperty.enumValueIndex == showIfAttribute.enumValue)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIfAttribute = (ShowIfEnumAttribute)attribute;
        SerializedProperty enumProperty = property.serializedObject.FindProperty(showIfAttribute.enumVariable);

        if (enumProperty != null && enumProperty.enumValueIndex == showIfAttribute.enumValue)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        return 0;
    }
}
#endif