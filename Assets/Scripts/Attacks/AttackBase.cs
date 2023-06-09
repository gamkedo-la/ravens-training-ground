using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackCosts { tier0 = 0, tier1 = 4, tier2 = 8, tier3 = 12, tier4 = 15, tier5 = 18, tier6 = 21 }
public enum ResourceType { Mana, Health }
public enum CastType { Friendly, Enemy }
public enum TargetType { SingleTarget, AOE }
public enum EffectType { Charms, Physical, DarkArts, Transfiguration, Ancient, Potions, None }
public enum AttackExcersion { LightAttack, MediumAttack, DefaultAttack }
[CreateAssetMenu(fileName ="Attack",menuName ="Actions/Attack")]
public class AttackBase : ScriptableObject
{
    public string AttackName;
    public string AttackDescription;
    public AttackCosts cost;
    public ResourceType resourceType;
    public CastType castType;
    public TargetType targetType;
    public EffectType effectType;
    public List<Affinity> affinities;
    public AttackExcersion attackExcersion;

    Battle battle;

    public bool AttemptAttack(Unit caster,Unit target)
    {
        if(resourceType == ResourceType.Mana)
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
        else if(resourceType == ResourceType.Health)
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

        Attack(caster, target);
        caster.anim.SetTrigger("Attack");
        return true;
    }

    void Attack(Unit caster,Unit target)
    {

        float damageType = GetEffectModifier(caster);
        float equipmentModifier = GetEquipmentModifier(caster);
        float excersionModifier = GetAttackExcersersion();

        float damage = (((Mathf.Sqrt(equipmentModifier) * Mathf.Sqrt(damageType)) + (caster.CurrentLevel * 1.25f)) * caster.attackMultiplier * Random.Range(.95f, 1.1f)) * excersionModifier;
        if (castType == CastType.Friendly) {
            damage = 0;
        }

        damage *= GetCriticalChanceMultiplier(caster);

        damage = CalculateTargetResistances(target, damage);

        damage = CalculateTargetWeaknesses(target, damage);

        Debug.Log($"{caster.Name} is casting attack {name} on {target.Name}");

        target.TakeDamage(damage);

        //battle = GameObject.Find("Battle").GetComponent<Battle>();

        //battle.ResolvingATurn(damage, this);
    }


    float GetEffectModifier(Unit caster)
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
    float CalculateTargetResistances(Unit target,float damage)
    {
        foreach(Affinity affinity in affinities)
        {
            foreach(Affinity resistance in target.resistances)
            {
                if (affinity == resistance)
                    damage /= 2;
            }
        }
        return damage;
    }
    float CalculateTargetWeaknesses(Unit target, float damage)
    {
        foreach (Affinity affinity in affinities)
        {
            foreach (Affinity weaknesses in target.weaknesses)
            {
                if (affinity == weaknesses)
                    damage *= 2;
            }
        }
        return damage;
    }
    float GetEquipmentModifier(Unit caster)
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

    float GetCriticalChanceMultiplier(Unit caster)
    {
        float chanceForCritial = Random.Range(0, 100);
        float criticalChance = caster.Finesse + caster.FinesseEquipment;

        if (chanceForCritial <= criticalChance)
            return 2;
        else
            return 1;
    }
}
