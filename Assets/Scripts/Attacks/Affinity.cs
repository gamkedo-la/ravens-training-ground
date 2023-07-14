using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AffinityType
{
    Charms, Physical, DarkArts, Transfiguration, Ancient, Potions, None
}
[CreateAssetMenu(fileName = "Affinity", menuName = "Affinity")]


public class Affinity : ScriptableObject
{
    public AffinityType affinityType;
}
