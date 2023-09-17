using System.Collections;
using System.Collections.Generic;
using Character.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Expendable", menuName = "Actions/Expendable")]

public class Expendable : InventoryItemBase
{
    //affect one for the party
    public bool solo, party;
    //increases or decreases stats
    public bool boost, detriment;
    //reincarnate
    public bool reincarnate;
    //specific stat affected
    public bool magicPoints, health, attack, defense;
    //affecting a direct number (like 50 health) or a percent (20% attack increase)
    public int specificAmount, percentageAmount;
    //only useable in battle (others can be used outside of battle)
    public bool onlyUsedInBattle;
}
