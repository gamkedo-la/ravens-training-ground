using Character.Stats;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack",menuName ="Actions/Attack")]
public class AttackBase : AbilityBase
{

    public void Attack(Unit caster,Unit target,float attackValue)
    {
        float damageType = GetEffectModifier(caster);
        float equipmentModifier = GetEquipmentModifier(caster);
        float excersionModifier = GetAttackExcersersion();

        attackValue = (((Mathf.Sqrt(equipmentModifier) * Mathf.Sqrt(damageType)) + (caster.CurrentLevel * 1.25f)) * caster.attackMultiplier * Random.Range(.95f, 1.1f)) * excersionModifier;
        if (castType == CastType.Friendly)
        {
            attackValue = 0;
        }

        attackValue = GetCriticalChanceMultiplier(caster,attackValue);

        attackValue = CalculateTargetModifiers(target,attackValue);

        target.GetComponent<Health>().TakeDamage(this.GameObject(), (int) attackValue);
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
