using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Potion of Resolve increases the attack of the effected unit by 50% for 3 turns
/// </summary>
public class PotionOfResolve : IStatusEffect
{
    public string StatusEffectName = "Potion of Resolve";
    public string StatusEffectDescription = "Increases attack by 50% for 3 turns";

    private float attackMultiplier = 1.5f;
    private int attackBonusTurnCount = 3;

    private float originalAttackMultiplier;
    private int originalAttackBonusTurnCount;

    public void ApplyEffect(Unit affectedCombatUnit) {
        Object.Instantiate(affectedCombatUnit.powerUpParticle, affectedCombatUnit.gameObject.transform.position, affectedCombatUnit.gameObject.transform.rotation);
        originalAttackMultiplier = affectedCombatUnit.attackMultiplier;
        originalAttackBonusTurnCount = affectedCombatUnit.attackBonusTurnCount;
        affectedCombatUnit.attackMultiplier = attackMultiplier;
        affectedCombatUnit.attackBonusTurnCount = attackBonusTurnCount;
    }

    public void RemoveEffect(Unit affectedCombatUnit) {
        affectedCombatUnit.attackMultiplier = originalAttackMultiplier;
        affectedCombatUnit.attackBonusTurnCount = originalAttackBonusTurnCount;
    }
}
