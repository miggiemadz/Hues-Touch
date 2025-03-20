using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemyai2))] 
public class EnemyAI2Editor : Editor {
    SerializedProperty enemyTypeProperty;
    SerializedProperty projectileProperty;

    private void OnEnable() {
        enemyTypeProperty = serializedObject.FindProperty("enemyType");
        projectileProperty = serializedObject.FindProperty("projectile");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update(); // Sync with the actual object

        // ✅ Draw all default properties **except projectile**
        DrawPropertiesExcluding(serializedObject, "projectile");

        // ✅ Show projectile field ONLY if the enemy type is Ranged
        if ((Enemyai2.EnemyType)enemyTypeProperty.enumValueIndex == Enemyai2.EnemyType.Ranged) {
            EditorGUILayout.PropertyField(projectileProperty);
        }

        serializedObject.ApplyModifiedProperties(); // Apply changes to the object
    }
}
