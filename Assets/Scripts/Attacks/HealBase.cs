using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Heal", menuName = "Actions/Heal")]
public class HealBase : AbilityBase
{
    public void Heal(Unit caster, Unit target, float healValue)
    {
        float healType = GetEffectModifier(caster);
        float equipmentModifier = GetEquipmentModifier(caster);

        healValue = (((Mathf.Sqrt(equipmentModifier) * Mathf.Sqrt(healType)) + (caster.CurrentLevel * 1.25f)) * caster.attackMultiplier * Random.Range(.95f, 1.1f));

        healValue = GetCriticalChanceMultiplier(caster, healValue);

        healValue = CalculateTargetModifiers(target, healValue);

        target.Heal(healValue);
    }
}
