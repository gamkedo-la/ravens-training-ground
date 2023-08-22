using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTester : MonoBehaviour
{
    public Unit caster;
    public AbilityBase abilityToUse;
    public List<Unit> targets;

    public void UseAbility()
    {
        if(caster == null) 
        {
            Debug.LogError("Missing Caster");
            return;
        }
        if (abilityToUse == null)
        {
            Debug.LogError("Missing Ability");
            return;
        }
        if(targets.Count == 0)
        {
            Debug.LogError("Missing Target(s)");
            return;
        }

        abilityToUse.UseAbility(caster,targets);
    }
}
