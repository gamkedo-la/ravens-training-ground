using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Need to have all enemies have '?' until player attacks them, then turn it off
    //bool[Trans, Charms, Phys, DarkArts, Ancient]
    public static bool[] Cherufe, Fresno, Jersey, Miniwashitu, Salem, Squonk;

    public static List<string> inCurrentParty = new List<string>();
    public static List<string> charactersNotInParty = new List<string>();

    public static List<Color> colorsInParty = new List<Color>();
    public static List<Color> colorsNotInParty = new List<Color>();

    public static float avgPlayerLevelPerPlayerInParty;
    //These are collected from 'RoamingMonster.cs' when a player runs into a monster, it populates these lists and is cleared out at the end of 'Battle.cs'
    public static int enemyCount;
    public static List<string> enemiesInThisFight = new List<string>();
    //
    public static bool initiativeSiezedByEnemy, initiativeSiezedByPlayer;

    public void Start()
    {
        inCurrentParty.Clear();
        charactersNotInParty.Clear();
        colorsInParty.Clear();
        colorsNotInParty.Clear();

        inCurrentParty.Add("Dani");
        inCurrentParty.Add("Erebus");
        inCurrentParty.Add("Sophie");
        inCurrentParty.Add("Theo");

        charactersNotInParty.Add("Tristan");
        charactersNotInParty.Add("Phoebe");

        colorsInParty.Add(new Color(255,176,0));
        colorsInParty.Add(new Color(83, 0, 255));
        colorsInParty.Add(new Color(0,170, 255));
        colorsInParty.Add(new Color(23,183,0));

        colorsNotInParty.Add(new Color(0, 0, 0));
        colorsNotInParty.Add(new Color(255,0, 0));
    }
}
