using ElectricDrill.SimpleRpgHealth;
using UnityEditor;
using UnityEngine;

namespace ElectricDrill.SimpleRpgCore.CstmEditor
{
    [CustomEditor(typeof(EntityHealth))]
    public class EntityHealthEditor : Editor
    {
        SerializedProperty healthCanBeNegative;
        SerializedProperty deathThreshold;
        SerializedProperty useClassMaxHp;
        SerializedProperty baseMaxHp;
        SerializedProperty healthAttributesScaling;
        SerializedProperty totalMaxHp;
        SerializedProperty hp;
        SerializedProperty healAmountModifier;
        SerializedProperty lifestealConfig;
        SerializedProperty preDmgInfoEvent;
        SerializedProperty takenDmgInfoEvent;
        SerializedProperty gainedHealthEvent;
        SerializedProperty lostHealthEvent;
        SerializedProperty entityDiedEvent;
        SerializedProperty preHealEvent;
        SerializedProperty entityHealedEvent;

        void OnEnable()
        {
            healthCanBeNegative = serializedObject.FindProperty("healthCanBeNegative");
            deathThreshold = serializedObject.FindProperty("deathThreshold");
            useClassMaxHp = serializedObject.FindProperty("useClassMaxHp");
            baseMaxHp = serializedObject.FindProperty("baseMaxHp");
            healthAttributesScaling = serializedObject.FindProperty("healthAttributesScaling");
            totalMaxHp = serializedObject.FindProperty("totalMaxHp");
            hp = serializedObject.FindProperty("hp");
            healAmountModifier = serializedObject.FindProperty("healAmountModifierStat");
            lifestealConfig = serializedObject.FindProperty("lifestealConfig");
            preDmgInfoEvent = serializedObject.FindProperty("preDmgInfoEvent");
            takenDmgInfoEvent = serializedObject.FindProperty("takenDmgInfoEvent");
            gainedHealthEvent = serializedObject.FindProperty("gainedHealthEvent");
            lostHealthEvent = serializedObject.FindProperty("lostHealthEvent");
            entityDiedEvent = serializedObject.FindProperty("entityDiedEvent");
            preHealEvent = serializedObject.FindProperty("preHealEvent");
            entityHealedEvent = serializedObject.FindProperty("entityHealedEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw the default inspector, including the script reference
            DrawDefaultInspector();

            // Draw the healthCanBeNegative property
            EditorGUILayout.PropertyField(healthCanBeNegative);

            // Conditionally hide deathThreshold based on healthCanBeNegative
            if (healthCanBeNegative.boolValue)
            {
                EditorGUILayout.PropertyField(deathThreshold);
            }

            // Draw the useClassMaxHp property
            
            EditorGUILayout.PropertyField(useClassMaxHp);

            // Conditionally set the maxHp property to read-only based on useClassMaxHp
            if (useClassMaxHp.boolValue)
            {
                baseMaxHp.FindPropertyRelative("isReadOnly").boolValue = true;
            }
            else
            {
                baseMaxHp.FindPropertyRelative("isReadOnly").boolValue = false;
            }

            // Draw the base maxHp property
            EditorGUILayout.PropertyField(baseMaxHp);
            
            // Draw the healthAttributesScaling property
            EditorGUILayout.PropertyField(healthAttributesScaling, new GUIContent("(o) Health Attributes Scaling"));

            // Total max hp is read-only
            totalMaxHp.FindPropertyRelative("isReadOnly").boolValue = true;
            
            // Draw the total maxHp property
            EditorGUILayout.PropertyField(totalMaxHp);
            
            // Draw the hp property
            EditorGUILayout.PropertyField(hp);

            // Draw the healAmountModifier property with a label
            EditorGUILayout.PropertyField(healAmountModifier, new GUIContent("(o) Heal Amount Modifier"));
            
            // Draw the lifestealConfig property
            EditorGUILayout.PropertyField(lifestealConfig);

            // Group the game events in a foldout group
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(preDmgInfoEvent);
            EditorGUILayout.PropertyField(takenDmgInfoEvent);
            EditorGUILayout.PropertyField(gainedHealthEvent);
            EditorGUILayout.PropertyField(lostHealthEvent);
            EditorGUILayout.PropertyField(entityDiedEvent);
            EditorGUILayout.PropertyField(preHealEvent);
            EditorGUILayout.PropertyField(entityHealedEvent);
            EditorGUI.indentLevel--;

            EditorGUILayout.HelpBox("(o): optional", MessageType.Info);

            serializedObject.ApplyModifiedProperties();
        }
    }
}