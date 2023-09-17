using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> ravensInventory, ravensClothing;
    public List<int> ravensStock, playerStock;
    // Start is called before the first frame update
    void Start()
    {
        ravensClothing.Add("Wand of the Undead");
        ravensClothing.Add("Chaos Core");
        ravensClothing.Add("Dragonwrath Wood");
        ravensClothing.Add("Wand of Glory");
        ravensClothing.Add("Savage Cane");

        ravensClothing.Add("Baseball Hat");
        ravensClothing.Add("Floppy Witches Hat");
        ravensClothing.Add("Tall Wizard Hat");
        ravensClothing.Add("Hood");
        ravensClothing.Add("Morgan le Fay");

        ravensClothing.Add("Nightfall Jacket");
        ravensClothing.Add("Thundersoul Shirt");
        ravensClothing.Add("Lifesoul Binder");
        ravensClothing.Add("Ancient Warrior Vest");
        ravensClothing.Add("Phantom Turtleneck ");

        ravensClothing.Add("Doomshadow Jeans");
        ravensClothing.Add("Cataclysm Trousers");
        ravensClothing.Add("Nexus Skirt");
        ravensClothing.Add("Plated Trousers");
        ravensClothing.Add("Skirt of Blight");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
