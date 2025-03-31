using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemyai2))] 
public class EnemyAI2Editor : Editor {
    SerializedProperty enemyTypeProperty;
    SerializedProperty projectileProperty;
    SerializedProperty MeleeDamageProperty;

    private void OnEnable()
    {
        if (target == null)
        {
            return;
        }
        enemyTypeProperty = serializedObject.FindProperty("enemyType");
        projectileProperty = serializedObject.FindProperty("projectile");
        MeleeDamageProperty = serializedObject.FindProperty("MeleeDamage");
    }

    public override void OnInspectorGUI() {
        if (target == null)
        {
            return;
        }
        serializedObject.Update(); // Sync with the actual object

        // ✅ Draw all default properties **except projectile**
        DrawPropertiesExcluding(serializedObject, "projectile", "MeleeDamage");
        


        // ✅ Show projectile field ONLY if the enemy type is Ranged
        if ((Enemyai2.EnemyType)enemyTypeProperty.enumValueIndex == Enemyai2.EnemyType.Ranged)
        {
            EditorGUILayout.PropertyField(projectileProperty);
        }
        else
        {
            EditorGUILayout.PropertyField(MeleeDamageProperty);
        }
        serializedObject.ApplyModifiedProperties(); // Apply changes to the object
    }
}
