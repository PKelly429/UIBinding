using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DataBinding.Editor
{
    [CustomEditor(typeof(UIBinding))]
    public class UIBindingInspector : UnityEditor.Editor
    {
        // Properties
        public UIBinding targetBinding;
        public static List<string> _typeDisplays;
        public static List<BinderTypePair> _typesAndNames = new List<BinderTypePair>();
        public static Dictionary<Type, BinderTypePair> _typeNameLookup = new Dictionary<Type, BinderTypePair>();

        private ListView _bindingsListView;

        private SerializedProperty _bindingsList;
        
        private const string _addNewTypeText = "Add New Binding ...";
        
        // Visual Assets
        #region Visual Assets
        public VisualTreeAsset BaseWindow;
        #endregion
        
        // Styles
        #region Styles
        public StyleSheet StyleSheetBase;
        #endregion
        
        // Class Names
        #region Class Names
        private const string _uiBindingClassName = "ui-binding";
        private const string _uiBindingPopupFieldLabel = "addNewBindingPopupField";
        private const string _binderClassName = "ui-binding-binder";
        private const string _binderHeaderClassName = ".ui-binding-header";
        #endregion
        
        private VisualElement _root;
        public override VisualElement CreateInspectorGUI()
        {
            serializedObject.Update();
            Undo.RecordObject(target, "UI Binder Updated");

            Initialization();
            DrawScriptField(_root);
            PopulateBindingTypes();
            DrawBinderTypeField(_root);
            DrawBindingsList(_root);
            DrawBottomBar(_root);
            
            serializedObject.ApplyModifiedProperties();
            return _root;
        }

        protected virtual void Initialization()
        {
            targetBinding = (UIBinding)target;

            bool foundNullBinder = false;
            if (targetBinding.binders == null)
            {
                targetBinding.binders = new List<AbstractBinder>();
                serializedObject.Update();
            }
            for(int i=targetBinding.binders.Count-1; i>=0; i--)
            {
                if (targetBinding.binders[i] == null)
                {
                    targetBinding.binders.RemoveAt(i);
                    foundNullBinder = true;
                }
            }

            if (foundNullBinder) serializedObject.ApplyModifiedProperties();

            _bindingsList = serializedObject.FindProperty("binders");
            
            _root = BaseWindow.Instantiate();
            _root.styleSheets.Add(StyleSheetBase);
        }
        
        protected virtual void DrawScriptField(VisualElement root)
        {
            SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");
            PropertyField scriptField = new PropertyField(scriptProperty);
            scriptField.SetEnabled(false);
            root.Insert(0, scriptField);
        }

        protected void PopulateBindingTypes()
        {
            if (_typeDisplays == null)
            {
                _typeDisplays = new List<string>();
            }
            
            List<System.Type> types = (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where assemblyType.IsSubclassOf(typeof(AbstractBinder))
                select assemblyType).ToList();
            
            _typeDisplays.Clear();
            _typeNameLookup.Clear();
            for (int i = 0; i < types.Count; i++)
            {
                if (types[i].IsAbstract) continue;
                
                BinderTypePair _newType = new BinderTypePair();
                _newType.Type = types[i];
                BinderAttribute attribute = types[i].GetCustomAttributes(false).OfType<BinderAttribute>().FirstOrDefault();
                _newType.Name = attribute != null ? attribute.name : $"{types[i].Name}";
                _newType.Path = attribute != null ? attribute.Path : $"Misc/{types[i].Name}";
                if (string.IsNullOrEmpty(_newType.Name) || _newType.Name == "AbstractBinder") 
                {
                    continue;
                }

                _typesAndNames.Add(_newType);
                _typeNameLookup.Add(_newType.Type, _newType);
            }

            _typesAndNames = _typesAndNames.OrderBy(t => t.Path).ToList();

            _typeDisplays.Add(_addNewTypeText);
            for (int i = 0; i < _typesAndNames.Count; i++)
            {
                _typeDisplays.Add(_typesAndNames[i].Path);
            }
        }

        protected void DrawBinderTypeField(VisualElement root)
        {
            var content = root.Q<VisualElement>("content");
            PropertyField binderTypeField = new PropertyField(serializedObject.FindProperty("bindingType"));
            content.Insert(0, binderTypeField);
        }

        protected void DrawBindingsList(VisualElement root)
        {
            _bindingsListView = root.Q<ListView>("bindings");
            SetBindingListData();
            
            _bindingsListView.fixedItemHeight = 25f;
            _bindingsListView.selectionType = SelectionType.Single;
            _bindingsListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            _bindingsListView.allowRemove = true;
            _bindingsListView.itemIndexChanged += (oldIndex, newIndex) => SetBindingListData();
            _bindingsListView.makeItem = () => new PropertyField();
            _bindingsListView.bindItem = (element, index) =>
            {
                PropertyField binderField = (element as PropertyField);
                if(binderField == null) return;
                
                binderField.AddToClassList(_binderClassName);
                var bindingProperty = _bindingsList.GetArrayElementAtIndex(index);
                binderField.label = $"{_typeNameLookup[targetBinding.binders[index].GetType()].Name}";
                binderField.bindingPath = bindingProperty.propertyPath;
                binderField.Bind(serializedObject);
                
                ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator((evt) =>
                {
                    FillContextualMenu(evt.menu, index);
                });
                binderField.AddManipulator(contextualMenuManipulator);
            };
        }
        
        protected virtual void FillContextualMenu(DropdownMenu menu, int index)
        {
            menu.ClearItems();
            menu.AppendAction("Remove", action => RemoveBinding(index), DropdownMenuAction.Status.Normal);
        }

        private void SetBindingListData()
        {
            serializedObject.Update();
            _bindingsListView.itemsSource = targetBinding.binders;
            _bindingsListView.Rebuild();
        }

        protected void DrawBottomBar(VisualElement root)
        {
            PopupField<string> addNewBinderPopupField =
                new PopupField<string>(_uiBindingPopupFieldLabel, _typeDisplays, 0);
            addNewBinderPopupField.Q<Label>().style.display = DisplayStyle.None;
            addNewBinderPopupField.style.flexGrow = 1;
            root.Add(addNewBinderPopupField);
            
            addNewBinderPopupField.RegisterValueChangedCallback(evt =>
            {
                int newItem = _typeDisplays.IndexOf(evt.newValue);
                if (newItem >= 0)
                {
                    serializedObject.Update();
                    Undo.RecordObject(target, "Add New Binding");
                    int newFeedbackIndex = newItem - 1;
                    AddBinding(_typesAndNames[newFeedbackIndex].Type);
                    serializedObject.ApplyModifiedProperties();
                    PrefabUtility.RecordPrefabInstancePropertyModifications(targetBinding);
                    addNewBinderPopupField.SetValueWithoutNotify(_typeDisplays[0]);
                    serializedObject.Update();
                    // CacheFeedbacksListProperty();
                    SetBindingListData();
                    // RedrawFeedbacksList();
                }
            });
        }

        private void AddBinding(Type type)
        {
            AbstractBinder newBinder = (AbstractBinder)Activator.CreateInstance(type);
            targetBinding.binders.Add(newBinder);
            serializedObject.ApplyModifiedProperties();
        }

        private void RemoveBinding(int index)
        {
            targetBinding.binders.RemoveAt(index);
            serializedObject.ApplyModifiedProperties();
            SetBindingListData();
        }
    }

    public struct BinderTypePair
    {
        public System.Type Type;
        public string Name;
        public string Path;
    }
}
