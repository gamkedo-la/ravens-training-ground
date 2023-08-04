using Character.Stats;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    #region Ability Selection
    public AbilityBase SelectAbility(List<AbilityBase> abilities)
    {
        return SelectRandomAbility(abilities);
    }

    AbilityBase SelectRandomAbility(List<AbilityBase> abilities)
    {
        AbilityBase abilityBase = null;

        abilityBase = abilities[(int)Random.Range(0,abilities.Count-1)];

        return abilityBase;
    }
    #endregion
    #region Target Selection
    public List<Unit> SelectTarget(AbilityBase ability,Unit unit, List<Unit> possibleTargets)
    {
        List<Unit> targets = new List<Unit>();

        if(ability.targetType == TargetType.AOE)
        {
            if (ability.castType == CastType.Friendly)
                targets = GetFriendlyUnits(unit, possibleTargets);
            if (ability.castType == CastType.Enemy)
                targets = GetEnemyUnits(unit, possibleTargets); 

            return targets;
        }

        Unit targetedUnit = null;

        if (ability.castType == CastType.Friendly)
            targetedUnit = GetRandomFriendlyUnit(unit, possibleTargets);
        if (ability.castType == CastType.Enemy)
            targetedUnit = GetRandomEnemyUnit(unit, possibleTargets);

        targets.Add(targetedUnit);

        if (ability.targetType == TargetType.TargetAndNeighbors)
        {   
            targets.AddRange(targetedUnit.GetNeighborUnits());
        }


        //SelectRandomTarget(possibleTargets);

        return targets;
    }

    Unit SelectRandomTarget(List<Unit> possibleTargets)
    {
        Unit target = null;

        target = possibleTargets[(int)Random.Range(0, possibleTargets.Count - 1)];

        return target;
    }

    public List<Unit> GetEnemyUnits(Unit unit, List<Unit> units)
    {
        return units.Where(x => x.isAPlayer != unit.isAPlayer && x.currentState != Unit.UnitState.Unconscious).ToList();
    }
    public List<Unit> GetFriendlyUnits(Unit unit, List<Unit> units)
    {
        return units.Where(x => x.isAPlayer == unit.isAPlayer && x.currentState != Unit.UnitState.Unconscious).ToList();
    }

    public List<Unit> GetUnconciousPlayers(Unit unit, List<Unit> units)
    {
        return GetFriendlyUnits(unit,units).Where(x => x.currentState == Unit.UnitState.Unconscious).ToList();
    }

    public Unit GetLowestHealthEnemy(Unit unit, List<Unit> units)
    {
        List<Unit> enemies = GetEnemyUnits(unit,units);
        enemies.OrderBy(x => x.gameObject.GetComponent<Health>().GetCurrentHP()).ToList();
        return enemies[0];
    }

    public Unit GetRandomEnemyUnit(Unit unit,List<Unit> units)
    {
        List<Unit> enemies = GetEnemyUnits(unit, units);
        if (enemies.Count == 0)
        {
            Debug.Log($"Error: No enemies but received query for random enemy");
        }
        return enemies.Count == 0 ? null : enemies[Random.Range(0, enemies.Count)];
    }
    public Unit GetRandomFriendlyUnit(Unit unit, List<Unit> units)
    {
        List<Unit> consciousPlayers = GetFriendlyUnits(unit,units);
        if (consciousPlayers.Count == 0)
        {
            Debug.Log($"Error: No players conscious but received query for random player");
        }
        return consciousPlayers.Count == 0 ? null : consciousPlayers[Random.Range(0, consciousPlayers.Count)];
    }

    #endregion
}
