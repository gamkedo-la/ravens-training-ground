using Character.Stats;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public enum AttackCosts { tier0 = 0, tier1 = 4, tier2 = 8, tier3 = 12, tier4 = 15, tier5 = 18, tier6 = 21 }
public enum AttackExcersion { LightAttack, MediumAttack, DefaultAttack }
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
    public AttackExcersion attackExcersion;
    public TargetType targetType;
    public EffectType effectType;
    public List<Affinity> affinities;
    public List<Enhancement> enhancements;

    Battle battle;
    
    public float abilityValue = 0;
    public bool AttemptAbility(Unit caster, Unit target)
    {
        int casterCurrentMP = caster.GetComponent<Magic>().GetCurrentMP();
        int casterCurrentHP = caster.GetComponent<Health>().GetCurrentHP();
        if (resourceType == ResourceType.Mana)
        {
            if (casterCurrentMP >= (int)cost)
            {
                caster.GetComponent<Magic>().UseMagic((int)cost);
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
            //user cannot cast an attack that is equal to their hitpoints as the attack would kill them
            if (casterCurrentHP > (int)cost)
            {
                int deductCost = (int)cost;
                caster.GetComponent<Health>().TakeDamage(this.GameObject(), (int)cost);
            }
            else
            {
                //If an attack is chosen the caster doesn't have enough MP/HP for it, circle it back to choose a different attack
                // DetermineAttackFromList()
                return false;
            }
        }

        UseAbility(caster, target);

        return true;
    }

    public void UseAbility(Unit caster, List<Unit> targets)
    {
        foreach (Unit target in targets)
        {
            UseAbility(caster, target);

        }
    }

    public void UseAbility(Unit caster, Unit target)
    {
        Debug.Log($"{caster.Name} is casting attack {name} on {target.Name}");

        if (this.GetType() == typeof(AttackBase))
        {
            (this as AttackBase).Attack(caster, target,abilityValue);
        }
        if (this.GetType() == typeof(HealBase))
        {
            (this as HealBase).Heal(caster, target, abilityValue);
        }

        foreach (Enhancement enhancement in enhancements)
        {
            target.AddEnhancement(enhancement);
        }
    }
    //Claculates all modifiers that comes from the targets attributes
    public float CalculateTargetModifiers(Unit target,float value)
    {
        value = CalculateTargetResistances(target, value);

        value = CalculateTargetWeaknesses(target, value);

        return value;
    }
    public float GetEffectModifier(Unit caster)
    {
        switch (effectType)
        {
            case EffectType.Charms:
                return caster.GetComponent<BaseStats>().GetStat(Stat.Magic);
            case EffectType.Physical:
                return caster.GetComponent<BaseStats>().GetStat(Stat.Physical);
            case EffectType.DarkArts:
                return caster.GetComponent<BaseStats>().GetStat(Stat.Magic);
            case EffectType.Transfiguration:
                return caster.GetComponent<BaseStats>().GetStat(Stat.Magic);
            case EffectType.Ancient:
                return caster.GetComponent<BaseStats>().GetStat(Stat.Magic);
            case EffectType.Potions:
                return caster.GetComponent<BaseStats>().GetStat(Stat.Magic);
            case EffectType.None:
                return caster.GetComponent<BaseStats>().GetStat(Stat.Magic);
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

    float GetAttackExcersersion()
    {
        switch (attackExcersion)
        {
            case AttackExcersion.DefaultAttack:
                return .6f;
            case AttackExcersion.LightAttack:
                return 1f;
            case AttackExcersion.MediumAttack:
                return 1.4f;
            default:
                return 0f;
        }
    }
    public float GetCriticalChanceMultiplier(Unit caster, float value)
    {
        float chanceForCritial = Random.Range(0, 100);
        float criticalChance = caster.GetComponent<BaseStats>().GetStat(Stat.Finesse) + caster.FinesseEquipment;

        if (chanceForCritial <= criticalChance)
            return value * 2;
        else
            return value * 1;
    }
}
