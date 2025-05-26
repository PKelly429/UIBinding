using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DataBinding.Editor
{
    [CustomPropertyDrawer(typeof(StringBinding))]
    public class StringBindingDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement element = new VisualElement();

            float margin = 2;
            float padding = 4;
            
            var helpBox = new VisualElement()
            { 
                style =
                {
                    flexDirection = FlexDirection.Row, 
                    alignItems = Align.Center, 
                    marginBottom = margin,
                    marginRight = margin,
                    marginLeft = margin,
                    marginTop = margin,
                    paddingTop = padding,
                    paddingBottom = padding,
                    paddingRight = padding,
                    paddingLeft = padding
                } 
            };
            helpBox.AddToClassList("unity-box");
            var label = new Label("Use '{0}' within the text area to add bindable parameters");
            helpBox.Add(label);
            
            element.Add(helpBox);
            
            PropertyField text = new PropertyField(property.FindPropertyRelative("formattedString"));
            PropertyField values = new PropertyField(property.FindPropertyRelative("parameters"));

            element.Add(text);
            element.Add(values);
            
            return element;
        }
        
        
    }
}
