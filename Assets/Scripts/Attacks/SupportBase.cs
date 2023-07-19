using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportBase : AbilityBase
{
    public List<Enhancement> enhancements;
    public void Cast(Unit caster, List<Unit> targets)
    {
        foreach (Unit target in targets)
        {
            Cast(caster, target);
        }
    }

    public void Cast(Unit caster, Unit target)
    {
        foreach(Enhancement enhancement in enhancements) 
        {
            target.AddEnhancement(enhancement);
        }

        Debug.Log($"{caster.Name} is casting attack {name} on {target.Name}");
    }
}
