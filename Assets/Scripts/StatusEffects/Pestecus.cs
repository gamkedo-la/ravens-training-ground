using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Increases health by amount to recover
/// </summary>
public class Pestecus : IStatusEffect
{
    public string StatusEffectName = "Pestecus";
    public string StatusEffectDescription = "Increases health by amount to recover";

    public void ApplyEffect(Unit affectedCombatUnit) {
        Object.Instantiate(affectedCombatUnit.powerUpParticle, affectedCombatUnit.gameObject.transform.position, affectedCombatUnit.gameObject.transform.rotation);
        affectedCombatUnit.CurrentHP += affectedCombatUnit.healthToRecover;
        if (affectedCombatUnit.CurrentHP >= affectedCombatUnit.MaxHP) {
            affectedCombatUnit.CurrentHP = affectedCombatUnit.MaxHP;
        }
    }

    public void RemoveEffect(Unit affectedCombatUnit) {
        // 
    }
}
