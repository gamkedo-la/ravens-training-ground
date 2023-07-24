using Character.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Increases hitpoints by amount to recover
/// </summary>
public class Pestecus : IStatusEffect
{
    public string StatusEffectName = "Pestecus";
    public string StatusEffectDescription = "Increases health by amount to recover";

    public void ApplyEffect(Unit affectedCombatUnit) {
        Object.Instantiate(affectedCombatUnit.powerUpParticle, affectedCombatUnit.gameObject.transform.position, affectedCombatUnit.gameObject.transform.rotation);
        affectedCombatUnit.GetComponent<Health>().AddHealth( (int) affectedCombatUnit.healthToRecover);
    }

    public void RemoveEffect(Unit affectedCombatUnit) {
        // 
    }
}
