using System.Collections;
using System.Collections.Generic;
using Character.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Clothing", menuName = "Actions/Clothing")]
public class Clothing : InventoryItemBase
{
    //hand, chest, legs, head
    public string equipableSlot;

    public string gender;

    public string modifier;
    public float percentIncrease;
}
