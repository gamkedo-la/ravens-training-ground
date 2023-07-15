using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackExcersion { LightAttack, MediumAttack, DefaultAttack }

[CreateAssetMenu(fileName ="Attack",menuName ="Actions/Attack")]
public class AttackBase : AbilityBase
{

    public AttackExcersion attackExcersion;
    public void Attack(Unit caster,List<Unit> targets)
    {
        foreach(Unit target in targets)
        {
            Attack(caster,target);
        }
    }

    public void Attack(Unit caster, Unit target)
    {
        abilityValue = 0;

        CalculateCasterModifiers(caster);

        CalculateTargetModifiers(target);

        Debug.Log($"{caster.Name} is casting attack {name} on {target.Name}");

        target.TakeDamage(abilityValue);
    }
    void CalculateCasterModifiers(Unit caster)
    {
        float damageType = GetEffectModifier(caster);
        float equipmentModifier = GetEquipmentModifier(caster);
        float excersionModifier = GetAttackExcersersion();

        abilityValue = (((Mathf.Sqrt(equipmentModifier) * Mathf.Sqrt(damageType)) + (caster.CurrentLevel * 1.25f)) * caster.attackMultiplier * Random.Range(.95f, 1.1f)) * excersionModifier;
        if (castType == CastType.Friendly)
        {
            abilityValue = 0;
        }

        abilityValue *= GetCriticalChanceMultiplier(caster);
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
