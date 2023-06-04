using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region EnemyAffinitiesKnown
    //Need to have all enemies have '?' until player attacks them, then turn it off
    public static bool CherufeTrans, CherufeCharms, CherufePotions, CherufePhys, CherufeDA, CherufeAncient;
    public static bool FresnoTrans, FresnoCharms, FresnoPotions, FresnoPhys, FresnoDA, FresnoAncient;
    public static bool MiniwashituTrans, MiniwashituCharms, MiniwashituPotions, MiniwashituPhys, MiniwashituDA, MiniwashituAncient;
    public static bool PukwudgieTrans, PukwudgieCharms, PukwudgiePotions, PukwudgiePhys, PukwudgieDA, PukwudgieAncient;
    public static bool SoucouyantTrans, SoucouyantCharms, SoucouyantPotions, SoucouyantPhys, SoucouyantDA, SoucouyantAncient;
    public static bool SquonkTrans, SquonkCharms, SquonkPotions, SquonkPhys, SquonkDA, SquonkAncient;
    #endregion

    public static float DaniWandAttunement = 1, ErebusWandAttunement = 1, PhoebeWandAttunement = 1, TristanWandAttunement = 1;

    public static List<string> inCurrentParty = new List<string>();
   
    public void Start()
    {
        inCurrentParty.Add("Dani");
        inCurrentParty.Add("Erebus");
        inCurrentParty.Add("Theo");
        inCurrentParty.Add("Sophie");

        //Get rid of this, this is testing
        SquonkTrans = true;
        SoucouyantCharms = true;
        PukwudgiePotions = true;
        MiniwashituPhys = true;
        FresnoAncient = true;
    }
}
