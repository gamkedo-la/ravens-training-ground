using Character.Stats;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityTester))]
public class AbilityTesterCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AbilityTester abilityTesterScript = (AbilityTester)target;
        if(GUILayout.Button("Use Ability"))
        {
            abilityTesterScript.UseAbility();
        }
    }
}
