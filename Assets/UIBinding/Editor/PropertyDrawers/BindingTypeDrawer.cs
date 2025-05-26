using System;
using System.Linq;
using DataBinding;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BindingType))]
public class BindingTypeDrawer : PropertyDrawer 
{
    string[] typeNames, typeFullNames;

    void Initialize() 
    {
        if (typeFullNames != null) return;
            
        var filteredTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.IsDefined(typeof(BindableAttribute), false))
            .ToArray();
            
        typeNames = filteredTypes.Select(t => t.ReflectedType == null ? t.Name : $"t.ReflectedType.Name + t.Name").ToArray();
        typeFullNames = filteredTypes.Select(t => t.AssemblyQualifiedName).ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        Initialize();
        var typeIdProperty = property.FindPropertyRelative("assemblyQualifiedName");

        if (typeFullNames == null || typeFullNames.Length < 1)
        {
            EditorGUI.LabelField(position,label.text, "No Bindable Types");
            
            position = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.helpBox);
            EditorGUI.HelpBox(position, "Add the [Bindable] attribute to a class you wish to bind", MessageType.Info);
            return;
        }
        
        if (string.IsNullOrEmpty(typeIdProperty.stringValue)) 
        {
            typeIdProperty.stringValue = typeFullNames.First();
            property.serializedObject.ApplyModifiedProperties();
        }

        var currentIndex = Array.IndexOf(typeFullNames, typeIdProperty.stringValue);
        var selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, typeNames);
            
        if (string.IsNullOrEmpty(typeIdProperty.stringValue) || (selectedIndex >= 0 && selectedIndex != currentIndex))
        {
            typeIdProperty.stringValue = typeFullNames[selectedIndex];
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}