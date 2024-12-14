using UnityEditor;
using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth.CstmEditor
{
    [CustomPropertyDrawer(typeof(LifestealStatConfig))]
    public class LifestealStatConfigDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var lifestealStat = property.FindPropertyRelative("lifestealStat");
            var dmgStateSelector = property.FindPropertyRelative("dmgStateSelector");
            var lifestealSource = property.FindPropertyRelative("lifestealSource");

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var lifestealStatRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var dmgStateSelectorRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
            var lifestealSourceRect = new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + 2), position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(lifestealStatRect, lifestealStat, new GUIContent("Lifesteal Stat"));
            EditorGUI.PropertyField(dmgStateSelectorRect, dmgStateSelector, new GUIContent("Damage State Selector"));
            EditorGUI.PropertyField(lifestealSourceRect, lifestealSource, new GUIContent("Lifesteal Source"));

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 3 * (EditorGUIUtility.singleLineHeight + 2);
        }
    }    
}
