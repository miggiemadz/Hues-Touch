/*using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{
    // Serialized properties for the EnemyAI fields
    SerializedProperty sightRangeProperty;
    SerializedProperty attackRangeProperty;
    SerializedProperty retreatDistanceProperty;
    SerializedProperty timeBetweenAttacksProperty;

    private void OnEnable()
    {
        // Find serialized properties
        sightRangeProperty = serializedObject.FindProperty("sightRange");
        attackRangeProperty = serializedObject.FindProperty("attackRange");
        retreatDistanceProperty = serializedObject.FindProperty("retreatDistance");
        timeBetweenAttacksProperty = serializedObject.FindProperty("timeBetweenAttacks");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Sync with the actual object

        // Draw default properties
        EditorGUILayout.PropertyField(sightRangeProperty);
        EditorGUILayout.PropertyField(attackRangeProperty);
        EditorGUILayout.PropertyField(timeBetweenAttacksProperty);

        // Check if StayAway is attached to the GameObject
        EnemyAI enemyAI = (EnemyAI)target;
        if (enemyAI.GetComponent<StayAway>() != null)
        {
            // Show retreatDistance only if StayAway is attached
            EditorGUILayout.PropertyField(retreatDistanceProperty);
        }

        serializedObject.ApplyModifiedProperties(); // Apply changes to the object
    }
}*/