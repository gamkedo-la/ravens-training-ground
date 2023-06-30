using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackCosts { tier0 = 0, tier1 = 4, tier2 = 8, tier3 = 12, tier4 = 15, tier5 = 18, tier6 = 21 }
public enum ResourceType { Mana, Health }
public enum CastType { Friendly, Enemy }
public enum TargetType { SingleTarget, AOE }
public enum EffectType { Charms, Physical, DarkArts, Transfiguration, Ancient, None }
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

    public void AttemptAttack(Unit unit)
    {
        if(resourceType== ResourceType.Mana)
        {


            if (unit.CurrentMP >= (int)cost)
            {
                int deductCost = (int)cost;
                unit.CurrentMP -= deductCost;

                Attack(unit);
            }
            else
                return;
        }
        else if(resourceType== ResourceType.Health)
        {
            if (unit.CurrentHP >= (int)cost)
            {
                int deductCost = (int)cost;
                unit.CurrentHP -= deductCost;

                Attack(unit);
            }
            else
                return;
        }
    }

    void Attack(Unit unit)
    {

        float damageType = GetEffectModifier(unit);
        float equipmentModifier = GetEquipmentModifier(unit);
        float excersionModifier = GetAttackExcersersion();

        float damage = (((Mathf.Sqrt(equipmentModifier) * Mathf.Sqrt(damageType)) + (unit.CurrentLevel * 1.25f)) * unit.attackMultiplier * Random.Range(.95f, 1.1f)) * excersionModifier;

        battle = GameObject.Find("Battle").GetComponent<Battle>();

        Debug.LogWarning(damage);
        battle.ResolvingATurnModified(damage);
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
