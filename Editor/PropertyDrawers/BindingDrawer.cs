using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DataBinding.Editor
{
    [CustomPropertyDrawer(typeof(BindingField))]
    public class BindingDrawer : PropertyDrawer
    {
        private string _typeId;
        string[] _targetNames;

        private bool _hasRequiredType;
        private string _requiredType;

        protected virtual void Initialize(SerializedProperty bindingType, SerializedProperty targetProperty)
        {
            string typeId = bindingType.FindPropertyRelative("assemblyQualifiedName").stringValue;
        
            if (_targetNames != null && _typeId == typeId) return;
            _typeId = typeId;

            if (!string.IsNullOrEmpty(typeId))
            {
                try
                {
                    var type = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .First(t => t.AssemblyQualifiedName == typeId);
                    
                    Type targetType = typeof(AbstractBindableVariable);

                    try
                    {
                        //var attributes = targetProperty.GetAttributes<BindingTypeAttribute>(false);
                        var fieldInfo = GetFieldInfo(targetProperty);
                        BindingTypeAttribute attribute = fieldInfo.GetCustomAttribute<BindingTypeAttribute>(false);
                        if (attribute != null)
                        {
                            SetRequiredType(attribute.bindingType);
                            if (attribute.bindingType == typeof(Enum))
                            {
                                _targetNames = type.GetFields().Where(t => typeof(AbstractBindableVariable).IsAssignableFrom(t.FieldType))
                                    .Where(t => t.FieldType.BaseType.IsGenericType && t.FieldType.BaseType.GetGenericTypeDefinition() == typeof(BindableEnum<>))
                                    .Select(t => t.Name).ToArray();
                                return;
                            }
                            else
                            {
                                targetType = typeof(BindableVariable<>).MakeGenericType(attribute.bindingType);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // could not find attribute
                    }

                    _targetNames = type.GetFields().Where(t => targetType.IsAssignableFrom(t.FieldType)).Select(t => t.Name).ToArray();
                }
                catch (Exception e)
                {
                    typeId = null;
                    _targetNames = Array.Empty<string>();
                }
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var serializedObject = property.serializedObject;
            var typeProperty = serializedObject.FindProperty("bindingType");
            var targetProperty = property.FindPropertyRelative("property");
            
            Initialize(typeProperty, property);

            if (!string.IsNullOrEmpty(typeProperty.FindPropertyRelative("assemblyQualifiedName").stringValue)) 
            {
                var currentIndex = string.IsNullOrEmpty(targetProperty.stringValue) ? 0 : Array.IndexOf(_targetNames, targetProperty.stringValue);

                if (_targetNames == null || _targetNames.Length < 1)
                {
                    EditorGUI.LabelField(position, "Binding Field","No Matching Fields");

                    string helpMessage = _hasRequiredType ? $"Has Required Type:\nBinds to fields of type {_requiredType}" : "Add fields extending from BindableVariable<T>\n e.g. BindableFloat";
                    position = GUILayoutUtility.GetRect(new GUIContent(helpMessage), EditorStyles.helpBox);
                    EditorGUI.HelpBox(position, helpMessage, MessageType.Info);
                    
                    return;
                }
                
                var selectedIndex = EditorGUI.Popup(position, "Binding Field", currentIndex, _targetNames);
            
                if (String.IsNullOrEmpty(targetProperty.stringValue) || (selectedIndex >= 0 && selectedIndex != currentIndex)) 
                {
                    targetProperty.stringValue = _targetNames[selectedIndex];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                EditorGUI.HelpBox(position, "Select a binding Type", MessageType.Info);
            }
        }

        private void SetRequiredType(Type type)
        {
            _hasRequiredType = true;
            if (type == typeof(float))
            {
                _requiredType = "BindableFloat";
                return;
            }
            
            if (type == typeof(bool))
            {
                _requiredType = "BindableBool";
                return;
            }
            
            if (type == typeof(string))
            {
                _requiredType = "BindableString";
                return;
            }
            
            if (type == typeof(int))
            {
                _requiredType = "BindableInt";
                return;
            }
            
            if (type == typeof(Color))
            {
                _requiredType = "BindableColour";
                return;
            }
            
            if (type == typeof(Enum))
            {
                _requiredType = "BindableEnum<T> : Cannot use this generic class directly, make a concrete class for each enum\n e.g. BindableChoice : BindableEnum<Choice>";
                return;
            }
            
            if (type == typeof(Sprite))
            {
                _requiredType = "BindableSprite";
                return;
            }
            
            if (type == typeof(Transform))
            {
                _requiredType = "BindableTransform";
                return;
            }
            
            _hasRequiredType = false;
        }
        
        private static MethodInfo internal_GetFieldInfoAndStaticTypeFromProperty;
        private FieldInfo GetFieldInfo(SerializedProperty prop) {
            if (internal_GetFieldInfoAndStaticTypeFromProperty == null) {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    foreach (var t in assembly.GetTypes()) {
                        if (t.Name == "ScriptAttributeUtility") {
                            internal_GetFieldInfoAndStaticTypeFromProperty = t.GetMethod("GetFieldInfoAndStaticTypeFromProperty", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                            break;
                        }
                    }
                    if (internal_GetFieldInfoAndStaticTypeFromProperty != null) break;
                }
            }
            var p = new object[] { prop, null };
            var fieldInfo = internal_GetFieldInfoAndStaticTypeFromProperty.Invoke(null, p) as FieldInfo;
            return fieldInfo;
        }
    }
}