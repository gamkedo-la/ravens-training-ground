using Character.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heals affected unit to max HP
/// </summary>
public class PotionOfHealing : IStatusEffect
{
    public string StatusEffectName = "Potion of Healing";
    public string StatusEffectDescription = "Slightly heals one party member. There is an 80% chance they heal the weakest party member, 20% chance it randomly heals another person in the party";

    public void ApplyEffect(Unit affectedCombatUnit) {
        Object.Instantiate(affectedCombatUnit.powerUpParticle, affectedCombatUnit.gameObject.transform.position, affectedCombatUnit.gameObject.transform.rotation);
        affectedCombatUnit.GetComponent<Health>().HealToFull();
    }

    public void RemoveEffect(Unit affectedCombatUnit) {
        // Do nothing
    }
}
