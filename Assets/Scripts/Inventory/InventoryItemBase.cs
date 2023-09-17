using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemBase : ScriptableObject
{
    public string nameOfItem;
    public int Cost;
    public int ravenStock;
    public int playerStock;

    public string whatDoesItDo;
}
