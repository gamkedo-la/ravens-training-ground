using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum AttackCosts { tier0 = 0, tier1 = 4, tier2 = 8, tier3 = 12, tier4 = 15, tier5 = 18, tier6 = 21 }
public enum ResourceType { Mana, Health }
public enum CastType { Friendly, Enemy }
public enum TargetType { SingleTarget, AOE }
public enum EffectType { Charms, Physical, DarkArts, Transfiguration, Ancient, Potions, None }

public class AbilityBase : ScriptableObject
{
    public string AttackName;
    public string AttackDescription;
    public AttackCosts cost;
    public ResourceType resourceType;
    public CastType castType;
    public TargetType targetType;
    public EffectType effectType;
    public List<Affinity> affinities;

    Battle battle;
    
    public float abilityValue = 0;
    public bool AttemptAbility(Unit caster, Unit target)
    {
        if (resourceType == ResourceType.Mana)
        {
            if (caster.CurrentMP >= (int)cost)
            {
                int deductCost = (int)cost;
                caster.CurrentMP -= deductCost;
            }
            else
            {
                //If an attack is chosen the caster doesn't have enough MP/HP for it, circle it back to choose a different attack
                // DetermineAttackFromList()
                return false;
            }
        }
        else if (resourceType == ResourceType.Health)
        {
            //user cannot cast an attack that is equal to their health as the attack would kill them
            if (caster.CurrentHP > (int)cost)
            {
                int deductCost = (int)cost;
                caster.CurrentHP -= deductCost;
            }
            else
            {
                //If an attack is chosen the caster doesn't have enough MP/HP for it, circle it back to choose a different attack
                // DetermineAttackFromList()
                return false;
            }
        }
        
        if(this.GetType() == typeof(AttackBase)) 
        {
            (this as AttackBase).Attack(caster, target);
        }
        return true;
    }
    //Claculates all modifiers that comes from the targets attributes
    public void CalculateTargetModifiers(Unit target)
    {
        abilityValue = CalculateTargetResistances(target, abilityValue);

        abilityValue = CalculateTargetWeaknesses(target, abilityValue);
    }
    public float GetEffectModifier(Unit caster)
    {
        switch (effectType)
        {
            case EffectType.Charms:
                return caster.Magic;
            case EffectType.Physical:
                return caster.Physical;
            case EffectType.DarkArts:
                return caster.Magic;
            case EffectType.Transfiguration:
                return caster.Magic;
            case EffectType.Ancient:
                return caster.Magic;
            case EffectType.Potions:
                return caster.Magic;
            case EffectType.None:
                return caster.Magic;
            default:
                Debug.Log("Missing Type");
                return 0;

        }
    }
    public float CalculateTargetResistances(Unit target, float abilityValue)
    {
        foreach (Affinity affinity in affinities)
        {
            foreach (Affinity resistance in target.resistances)
            {
                if (affinity == resistance)
                    abilityValue /= 2;
            }
        }
        return abilityValue;
    }
    public float CalculateTargetWeaknesses(Unit target, float abilityValue)
    {
        foreach (Affinity affinity in affinities)
        {
            foreach (Affinity weaknesses in target.weaknesses)
            {
                if (affinity == weaknesses)
                    abilityValue *= 2;
            }
        }
        return abilityValue;
    }
    public float GetEquipmentModifier(Unit caster)
    {
        switch (effectType)
        {
            case EffectType.Charms:
                return 0;
            case EffectType.Physical:
                return caster.PhysicalEquipment;
            case EffectType.DarkArts:
                return 0;
            case EffectType.Transfiguration:
                return 0;
            case EffectType.Ancient:
                return 0;
            case EffectType.Potions:
                return 0;
            case EffectType.None:
                return 0;
            default:
                Debug.Log("Missing Type");
                return 0;

        }
    }
    public float GetCriticalChanceMultiplier(Unit caster)
    {
        float chanceForCritial = Random.Range(0, 100);
        float criticalChance = caster.Finesse + caster.FinesseEquipment;

        if (chanceForCritial <= criticalChance)
            return 2;
        else
            return 1;
    }
}
