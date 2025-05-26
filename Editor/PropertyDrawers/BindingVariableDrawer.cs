using System.Reflection;
using DataBinding;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AbstractBindableVariable), true)]
public class BindingVariableDrawer : PropertyDrawer
{
    private PropertyInfo propertyFieldInfo = null;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property.FindPropertyRelative("_value"), label);
    }
}