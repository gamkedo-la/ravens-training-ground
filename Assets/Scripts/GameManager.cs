using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Need to have all enemies have '?' until player attacks them, then turn it off
    //bool[Trans, Charms, Phys, DarkArts, Ancient]
    public static bool[] Cherufe, Fresno, Jersey, Miniwashitu, Salem, Squonk;

    public static List<string> inCurrentParty = new List<string>();
   
    public void Start()
    {
        inCurrentParty.Add("Dani");
        inCurrentParty.Add("Erebus");
        inCurrentParty.Add("Theo");
        inCurrentParty.Add("Sophie");
    }
}
