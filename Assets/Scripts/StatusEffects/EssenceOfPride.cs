using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Increases attack by 50%, defense by 50%, and keeps those stats for 3 turns
/// </summary>
public class EssenceOfPride : IStatusEffect 
{
    public string StatusEffectName = "EssenceOfPride";
    public string StatusEffectDescription = "Increases health by amount to recover";

    private float defenseMultiplier = .5f;
    private int defenseBonusTurnCount = 3;

    private float originalDefenseMultiplier;
    private int originalDefenseBonusTurnCount;

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

        originalDefenseMultiplier = affectedCombatUnit.defenseMultiplier;
        originalDefenseBonusTurnCount = affectedCombatUnit.defenseBonusTurnCount;
        affectedCombatUnit.defenseMultiplier = defenseMultiplier;
        affectedCombatUnit.defenseBonusTurnCount = defenseBonusTurnCount;
    }

    public void RemoveEffect(Unit affectedCombatUnit) {
        affectedCombatUnit.attackMultiplier = originalAttackMultiplier;
        affectedCombatUnit.attackBonusTurnCount = originalAttackBonusTurnCount;

        affectedCombatUnit.defenseMultiplier = originalDefenseMultiplier;
        affectedCombatUnit.defenseBonusTurnCount = originalDefenseBonusTurnCount;
    }
}
