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
    public AttackExcersion attackExcersion;

    Battle battle;

    public void AttemptAttack(Unit caster,Unit target)
    {
        if(resourceType == ResourceType.Mana)
        {


            if (caster.CurrentMP >= (int)cost)
            {
                int deductCost = (int)cost;
                caster.CurrentMP -= deductCost;
            } 
            else
                return;
        }
        else if(resourceType == ResourceType.Health)
        {
            if (caster.CurrentHP >= (int)cost)
            {
                int deductCost = (int)cost;
                caster.CurrentHP -= deductCost;
            }
            else
                return;
        }


        Attack(caster, target);
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
        battle = GameObject.Find("Battle").GetComponent<Battle>();

        battle.ResolvingATurn(damage, this);
    }


    float GetEffectModifier(Unit unit)
    {
        switch (effectType)
        {
            case EffectType.Charms:
                return unit.Magic;
            case EffectType.Physical:
                return unit.Physical;
            case EffectType.DarkArts:
                return unit.Magic;
            case EffectType.Transfiguration:
                return unit.Magic;
            case EffectType.Ancient:
                return unit.Magic;
            case EffectType.Potions:
                return unit.Magic;
            case EffectType.None:
                return unit.Magic;
            default:
                Debug.Log("Missing Type");
                return 0;

        }
    }
    float GetEquipmentModifier(Unit unit)
    {
        switch (effectType)
        {
            case EffectType.Charms:
                return 0;
            case EffectType.Physical:
                return unit.PhysicalEquipment;
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
}
