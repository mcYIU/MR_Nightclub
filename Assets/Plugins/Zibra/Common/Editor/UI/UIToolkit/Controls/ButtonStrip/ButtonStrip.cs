using System;
using System.Collections.Generic;
using System.Linq;
using com.zibra.common.Foundation.Editor;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.UIElements;

namespace com.zibra.common.Foundation.UIElements
{
    /// <summary>
    /// The button strip component let's you place buttons group with the labels or images.
    /// First and last buttons are styled to have round corners.
    /// Component is made to replace IMGUI Toolbar: <see
    /// href="https://docs.unity3d.com/ScriptReference/GUILayout.Toolbar.html"/>
    /// </summary>
    internal sealed class ButtonStrip : VisualElement
    {
        /// <exclude/>
        [UsedImplicitly]
        internal new class UxmlFactory : UxmlFactory<ButtonStrip, UxmlTraits>
        {
        }

        /// <summary>
        /// <exclude/>
        /// </summary>
        internal new class UxmlTraits : BindableElement.UxmlTraits {}

        /// <summary>
        /// ButtonStrip control Uss class name
        /// </summary>
        internal const string UssClassName = "zibraai-button-strip";

        private const string k_ButtonClassName = UssClassName + "__button";
        private const string k_ButtonLeftClassName = k_ButtonClassName + "--left";
        private const string k_ButtonMidClassName = k_ButtonClassName + "--mid";
        private const string k_ButtonRightClassName = k_ButtonClassName + "--right";
        private const string k_ButtonIconClassName = UssClassName + "__button-icon";
        private const string k_ButtonActiveClassNameDark = k_ButtonClassName + "--active-dark";
        private const string k_ButtonActiveClassNameLight = k_ButtonClassName + "--active-light";

        private readonly List<string> m_Choices = new List<string>();
        private readonly List<string> m_Labels = new List<string>();
        private readonly List<Button> m_Buttons = new List<Button>();

        /// <summary>
        /// Available Choices.
        /// </summary>
        internal IEnumerable<string> Choices => m_Choices;

        /// <summary>
        /// Available Labels.
        /// </summary>
        internal IEnumerable<string> Labels => m_Labels;

        private readonly TextField m_TextField;

        /// <summary>
        /// Current value. Should be one of the available <see cref="Choices"/>.
        /// </summary>
        internal string Value { get; set; }

        /// <summary>
        /// Action is called when one of the buttons clicked.
        /// </summary>
        internal event Action OnButtonClick;

        /// <summary>
        /// Creates ButtonStrip control with default choices.
        /// </summary>
        public ButtonStrip() : this(new[] { "LEFT", "MIDDLE", "RIGHT" })
        {
        }

        /// <summary>
        /// Creates ButtonStrip control with choices.
        /// </summary>
        /// <param name="choices">Available chaises.</param>
        internal ButtonStrip(IEnumerable<string> choices)
        {
            AddToClassList(UssClassName);
            UIToolkitEditorUtility.ApplyStyleForInternalControl(this, nameof(ButtonStrip));

            var collection = choices.ToList();
            m_Choices.AddRange(collection);
            m_Labels.AddRange(collection);
            RecreateButtons();

            m_TextField = new TextField { viewDataKey = "view-data" };
            m_TextField.style.display = DisplayStyle.None;
            Add(m_TextField);

            // This is only possible when data is restored.
            m_TextField.RegisterValueChangedCallback(e => { SetValue(e.newValue); });
        }

        /// <summary>
        /// Set current value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        internal void SetValue(string value)
        {
            SetValueWithoutNotify(value);
            OnButtonClick?.Invoke();
        }

        /// <summary>
        /// Set current value without triggering the <see cref="OnButtonClick"/> event.
        /// </summary>
        /// <param name="value"></param>
        internal void SetValueWithoutNotify(string value)
        {
            var index = m_Choices.IndexOf(value);
            if (index == -1)
                return;

            Value = value;
            m_TextField.SetValueWithoutNotify(value);
            ToggleButtonStates(m_Buttons[index]);
        }

        /// <summary>
        /// Add choice to the strip.
        /// </summary>
        /// <param name="choice">Choice value.</param>
        /// <param name="label">Choice label.</param>
        internal void AddChoice(string choice, string label)
        {
            m_Choices.Add(choice);
            m_Labels.Add(label);
            RecreateButtons();

            if (m_Choices.Count == 1)
                SetValueWithoutNotify(choice);
        }

        /// <summary>
        /// Remove all the buttons.
        /// </summary>
        internal void CleanUp()
        {
            foreach (var button in m_Buttons)
                button.RemoveFromHierarchy();

            m_Choices.Clear();
            m_Labels.Clear();
            m_Buttons.Clear();
        }

        private void RecreateButtons()
        {
            foreach (var button in m_Buttons)
                button.RemoveFromHierarchy();

            m_Buttons.Clear();
            for (var i = 0; i < m_Choices.Count; ++i)
            {
                var choice = m_Choices[i];
                string label = null;
                if (m_Labels.Count > i)
                    label = m_Labels[i];

                var button = new Button();
                button.AddToClassList(k_ButtonClassName);

                // Set button name for styling.
                button.name = choice;

                // Set tooltip.
                button.tooltip = choice;

                if (i == 0)
                    button.AddToClassList(k_ButtonLeftClassName);
                else if (i == m_Choices.Count - 1)
                    button.AddToClassList(k_ButtonRightClassName);
                else
                    button.AddToClassList(k_ButtonMidClassName);

                button.clicked += () =>
                { SetValue(choice); };

                m_Buttons.Add(button);

                Add(button);
                if (string.IsNullOrEmpty(label))
                {
                    var icon = new VisualElement();
                    icon.AddToClassList(k_ButtonIconClassName);
                    button.Add(icon);
                }
                else
                {
                    button.text = label;
                }
            }

            SetValueWithoutNotify(Value);
        }

        private void ToggleButtonStates(Button button)
        {
            foreach (var btn in m_Buttons)
            {
                btn.RemoveFromClassList(k_ButtonActiveClassNameDark);
                btn.RemoveFromClassList(k_ButtonActiveClassNameLight);
            }

            button.AddToClassList(EditorGUIUtility.isProSkin ? k_ButtonActiveClassNameDark
                                                             : k_ButtonActiveClassNameLight);
        }
    }
}
