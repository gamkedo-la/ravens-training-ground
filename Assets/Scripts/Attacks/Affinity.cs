using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AffinityType
{
    Charms, Physical, DarkArts, Transfiguration, Ancient
}
[CreateAssetMenu(fileName = "Affinity", menuName = "Affinity")]


public class Affinity : ScriptableObject
{
    public AffinityType affinityType;
}
