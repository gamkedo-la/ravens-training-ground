using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MathSign { Add,Subtract,Multiply,Divide};
public class Enhancement : MonoBehaviour
{
    public int numberOfTurns;
    
    public void RemoveEnhancement(List<Enhancement> enhancementsOnUnit)
    {
        enhancementsOnUnit.Remove(this);
    }
}
