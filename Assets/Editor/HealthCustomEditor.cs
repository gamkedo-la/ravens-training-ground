using Character.Stats;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Health))]
public class HealthCustomEditor : Editor
{
    [SerializeField] Unit killer;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Health healthScript = (Health)target;

        EditorGUILayout.LabelField("Death Debugging", EditorStyles.boldLabel);
        
        killer =EditorGUILayout.ObjectField("Killer", killer, typeof(Unit), true) as Unit;
        if (GUILayout.Button("Kill"))
        {
            if(killer == null)
            {
                Debug.LogError("Missing Killer");
                return;
            }

            healthScript.TakeDamage(killer,healthScript.GetCurrentHP());
        }
    }
}
