using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MathSign { Add,Subtract,Multiply,Divide};
public class Enhancement : MonoBehaviour
{
    Unit unitAttachedTo;
    
    public int numberOfTurns;
    public int turnCount;

    public void AddEnhancement(Unit unit)
    {
        unitAttachedTo = unit;
        unit.enhancements.Add(this);
    }
    public void ProgressEnhancement()
    {
        turnCount++;

        if (turnCount >= numberOfTurns)
        {
            RemoveEnhancement();
        }
    }

    public void RemoveEnhancement()
    {
        unitAttachedTo.enhancements.Remove(this);
    }
}
