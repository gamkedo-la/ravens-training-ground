using Character.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

public enum MathSign {Multiply,Divide, Add,Subtract};
public enum BufforDebuff { Buff,Debuff };
public enum Activation {Immediate, StartOfTurn, EndOfTurn};

public class EnhancementStatAmountArgs : EventArgs
{
    public EnhancementStatAmountArgs(float enhancementAmount, Stat effectedStat) {
        EnhancementAmount = enhancementAmount;
        EffectedStat = effectedStat;
    }

    public float EnhancementAmount { get; set; }
    public Stat EffectedStat { get; set; }
}

[CreateAssetMenu(fileName = "Enhancement", menuName = "Enhancement")]
public class Enhancement : ScriptableObject
{
    Unit unitAttachedTo;
    public Activation activation;
    public BufforDebuff buffOrDebuff;
    public Stat statToChange;
    [Header("Enhancement Variables")]
    public MathSign sign;
    public float effectiveness;
    [Header("Turn Variables")]
    public int numberOfTurns;
    public int turnCount;

/*    public float magicEnhancementAmount;
    public float physicalEnhancementAmount;
    public float finesseEnhancementAmount;
    public float agilityEnhancementAmount;*/

    public event EventHandler<EnhancementStatAmountArgs> OnEnhancementEvent;

    public void Initialize(Unit unit)
    {
        unitAttachedTo = unit;

        ApplyEnhancement();
    }
    public void ApplyEnhancement()
    {
        float stat = GetStat();

        unitAttachedTo.GetComponent<BaseStats>().RegisterEnhancementEvent(this);

        SetStat(AddOrSubtract(Calculate(stat)));
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
            case Stat.Magic:
                return unitAttachedTo.GetComponent<BaseStats>().GetStat(Stat.Magic);
            case Stat.Physical:
                return unitAttachedTo.GetComponent<BaseStats>().GetStat(Stat.Physical);
            case Stat.Finesse:
                return unitAttachedTo.GetComponent<BaseStats>().GetStat(Stat.Finesse);
            case Stat.Agility:
                return unitAttachedTo.GetComponent<BaseStats>().GetStat(Stat.Agility);
            default:
                return 0;
        }        
    }
    float AddOrSubtract(float input)
    {
        if (buffOrDebuff == BufforDebuff.Buff)
        {
            Debug.Log(input);
            return input;
        }
        else
        {
            Debug.Log(-input);
            return -input;
        }

    }
    void SetStat(float statModificationAmount)
    {
        switch (statToChange)
        {
            case Stat.Magic:
                OnEnhancementEvent(this, new EnhancementStatAmountArgs(statModificationAmount, Stat.Magic));
                //magicEnhancementAmount = statModificationAmount;
                break;
            case Stat.Physical:
                OnEnhancementEvent(this, new EnhancementStatAmountArgs(statModificationAmount, Stat.Physical));
                //physicalEnhancementAmount = statModificationAmount;
                break;
            case Stat.Finesse:
                OnEnhancementEvent(this, new EnhancementStatAmountArgs(statModificationAmount, Stat.Finesse));
                //finesseEnhancementAmount = statModificationAmount;
                break;
            case Stat.Agility:
                OnEnhancementEvent(this, new EnhancementStatAmountArgs(statModificationAmount, Stat.Agility));
                //agilityEnhancementAmount = statModificationAmount;
                break;
        }
    }
    public void RemoveEnhancement()
    {
        OnEnhancementEvent(this, new EnhancementStatAmountArgs(0, statToChange));
        unitAttachedTo.enhancements.Remove(this);
    }
}
