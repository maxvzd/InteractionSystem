using System;
using Items.ItemInterfaces;
using Items.Properties;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ItemProperties), true)]
    public class ItemPropertiesEditorScript : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            
            SerializedProperty guidProperty = serializedObject.FindProperty("prefabId");

            DrawPropertiesExcluding(serializedObject, guidProperty.name);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(guidProperty);
            EditorGUI.EndDisabledGroup();
            
            if (GUILayout.Button("Generate Guid", EditorStyles.miniButton) && string.IsNullOrEmpty(guidProperty.stringValue))
            {
                guidProperty.stringValue = Guid.NewGuid().ToString();
            }
                
            serializedObject.ApplyModifiedProperties();
        }
    }
}