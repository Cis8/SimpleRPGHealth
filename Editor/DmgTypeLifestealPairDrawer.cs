using ElectricDrill.SimpleRpgCore.Utils;
using UnityEditor;
using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth.CstmEditor
{
    [CustomPropertyDrawer(typeof(SerKeyValPair<LifestealStatConfig, DmgStateSelector>))]
    public class DmgTypeLifestealPairDrawer : PropertyDrawer
    {
        /*public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var keyProp = property.FindPropertyRelative("Key");
            var valueProp = property.FindPropertyRelative("Value");

            var keyRect = new Rect(position.x, position.y, position.width * 0.7f,
                EditorGUI.GetPropertyHeight(keyProp));
            var valueRect = new Rect(position.x + position.width * 0.72f, position.y,
                position.width * 0.28f, EditorGUIUtility.singleLineHeight);

            // Draw LifestealStatConfig
            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none, true);

            // Draw enum dropdown for value
            if (valueProp.propertyType == SerializedPropertyType.Enum)
            {
                valueProp.enumValueIndex = EditorGUI.Popup(valueRect, valueProp.enumValueIndex, valueProp.enumDisplayNames);
            }
            else
            {
                EditorGUI.LabelField(valueRect, "Not an enum");
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var keyProp = property.FindPropertyRelative("key");
            return EditorGUI.GetPropertyHeight(keyProp);
        }*/
    }
}