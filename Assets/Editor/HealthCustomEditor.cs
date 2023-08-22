using Character.Stats;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Health))]
public class HealthCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Health healthScript = (Health)target;
        if(GUILayout.Button("Kill"))
        {
            healthScript.TakeDamage(null,healthScript.GetCurrentHP());
        }
    }
}
