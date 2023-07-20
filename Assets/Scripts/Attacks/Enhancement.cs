using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public enum MathSign {Multiply,Divide, Add,Subtract};
public enum Activation {Immediate, StartOfTurn, EndOfTurn};
public enum StatToChange
{
    Magic, Physical, Agility, Finesse
}
[CreateAssetMenu(fileName = "Enhancement", menuName = "Enhancement")]
public class Enhancement : ScriptableObject
{
    Unit unitAttachedTo;
    public Activation activation;
    public StatToChange statToChange;
    [Header("Enhancement Variables")]
    public MathSign sign;
    public float effectiveness;
    [Header("Turn Variables")]
    public int numberOfTurns;
    public int turnCount;

    public void Initialize(Unit unit)
    {
        unitAttachedTo = unit;
    }
    public void ApplyEnhancement()
    {
        float stat = GetStat();

        SetStat(Calculate(stat));
    }
    public void ProgressTurn()
    {
        turnCount++;
        if (turnCount >= numberOfTurns)
        {
            RemoveEnhancement();
        }
    }
    float Calculate(float inputValue)
    {
        float outputValue = inputValue;

        switch (sign)
        {
            case MathSign.Add:
                outputValue += effectiveness;
                break;
            case MathSign.Subtract:
                outputValue -= effectiveness;
                break;
            case MathSign.Multiply:
                outputValue *= effectiveness;
                break;
            case MathSign.Divide:
                outputValue /= effectiveness;
                break;
        }

        return outputValue;
    }
    float GetStat()
    {
        switch (statToChange)
        {
            case StatToChange.Magic:
                return unitAttachedTo.Magic;
            case StatToChange.Physical:
                return unitAttachedTo.Physical;
            case StatToChange.Finesse:
                return unitAttachedTo.Finesse;
            case StatToChange.Agility:
                return unitAttachedTo.Agility;
            default:
                return 0;
        }        
    }
    void SetStat(float stat)
    {
        switch (statToChange)
        {
            case StatToChange.Magic:
                unitAttachedTo.Magic = stat;
                break;
            case StatToChange.Physical:
                unitAttachedTo.Physical = stat;
                break;
            case StatToChange.Finesse:
                unitAttachedTo.Finesse = stat;
                break;
            case StatToChange.Agility:
                unitAttachedTo.Agility = stat;
                break;
        }
    }
    public void RemoveEnhancement()
    {
        unitAttachedTo.enhancements.Remove(this);
    }
}
