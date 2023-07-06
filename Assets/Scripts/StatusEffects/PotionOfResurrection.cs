using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Brings affected combat unit back to life with half HP
/// </summary>
public class PotionOfResurrection : IStatusEffect
{
    public string StatusEffectName = "Potion of Resurrection";
    public string StatusEffectDescription = "Brings one party member back with 50% health";

    public float attackMultiplier;
    public float healthToRecover;
    public float defenseMultiplier;

    public int attackBonusTurnCount;
    public int defenseBonusTurnCount;


    private float originalAttackMultiplier;
    private int originalAttackBonusTurnCount;

    public void ApplyEffect(Unit affectedCombatUnit) {
        affectedCombatUnit.currentState = Unit.UnitState.Alive;
        affectedCombatUnit.CurrentHP = (affectedCombatUnit.MaxHP * .5f);
        Transform affectedCombatUnitTransform = affectedCombatUnit.transform; 
        // TODO - Fire event for unit add as combatant
        // Combatants.Add(deadCharacters[deadPlayerChosen]);

        Object.Instantiate(affectedCombatUnit.deathParticle, affectedCombatUnitTransform.position, affectedCombatUnitTransform.rotation);

        foreach (Transform child in affectedCombatUnitTransform) {
            Object.Instantiate(affectedCombatUnit.powerUpParticle, affectedCombatUnitTransform.position, affectedCombatUnitTransform.rotation);
            child.gameObject.SetActive(true);
        }
        affectedCombatUnit.anim.SetTrigger("Charge");
        // TODO - Fire event for unit add removed as dead
        // deadCharacters.Remove(deadCharacters[deadPlayerChosen]);
    }

    public void RemoveEffect(Unit currentCombatantUnit) {
        currentCombatantUnit.attackMultiplier = originalAttackMultiplier;
        currentCombatantUnit.attackBonusTurnCount = originalAttackBonusTurnCount;
    }
}
