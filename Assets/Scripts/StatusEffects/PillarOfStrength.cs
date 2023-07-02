using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Potion of Resolve increases the Defense of the effected unit by 50% for 3 turns
/// </summary>
public class PillarOfStrength : IStatusEffect
{
    public string StatusEffectName = "Pillar of Strength";
    public string StatusEffectDescription = "increases the Defense of the effected unit by 50% for 3 turns";

    private float defenseMultiplier = .5f;
    private int defenseBonusTurnCount = 3;

    private float originalDefenseMultiplier;
    private int originalDefenseBonusTurnCount;

    public void ApplyEffect(Unit affectedCombatUnit) {
        Object.Instantiate(affectedCombatUnit.powerUpParticle, affectedCombatUnit.gameObject.transform.position, affectedCombatUnit.gameObject.transform.rotation);
        originalDefenseMultiplier = affectedCombatUnit.defenseMultiplier;
        originalDefenseBonusTurnCount = affectedCombatUnit.defenseBonusTurnCount;
        affectedCombatUnit.defenseMultiplier = defenseMultiplier;
        affectedCombatUnit.defenseBonusTurnCount = defenseBonusTurnCount;
    }

    public void RemoveEffect(Unit affectedCombatUnit) {
        affectedCombatUnit.defenseMultiplier = originalDefenseMultiplier;
        affectedCombatUnit.defenseBonusTurnCount = originalDefenseBonusTurnCount;
    }
}
