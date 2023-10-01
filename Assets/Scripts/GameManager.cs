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

    public static bool floor2Unlocked, floor3Unlocked;

    public List<Expendable> playerInventory;
    public List<Clothing> playerClothes;

    //currency of choice for Ravens
    public static float shinyThings;

    //holding variables for inventory;
    public static int holdingMagic, holdingHealth;
    public static bool holdingDeath;
    public static Expendable itemName;
    public static Clothing clothingName;

    public static bool floor1Generated, floor2Generated;
    public static Vector3 player;
    public static Vector3[] party;

    private static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Start()
    {
        shinyThings += 100;

        inCurrentParty.Clear();
        charactersNotInParty.Clear();
        colorsInParty.Clear();
        colorsNotInParty.Clear();

        //Remove this before final build - second thought - keep this in, players can change this, but we'll start with 4
        inCurrentParty.Add("Dani");
        inCurrentParty.Add("Erebus");
        inCurrentParty.Add("Theo");
        inCurrentParty.Add("Tristan");

        colorsInParty.Add(new Color(255, 176, 0));
        colorsInParty.Add(new Color(83, 0, 255));
        colorsInParty.Add(new Color(0, 170, 255));
        colorsInParty.Add(new Color(255, 0, 0));


        /* For Final Build
        inCurrentParty.Add("Dani");

        charactersNotInParty.Add("Erebus");
        charactersNotInParty.Add("Theo");
        charactersNotInParty.Add("Tristan");
        charactersNotInParty.Add("Phoebe");
        charactersNotInParty.Add("Sophie");

        colorsInParty.Add(new Color(255,176,0));

        colorsNotInParty.Add(new Color(83, 0, 255));
        colorsNotInParty.Add(new Color(0, 170, 255));
        colorsNotInParty.Add(new Color(255, 0, 0));
        colorsNotInParty.Add(new Color(0, 0, 0));
        colorsNotInParty.Add(new Color(23, 183, 0));
        */
    }

    public static void ClearGameManagerDataFromInventory()
    {
        itemName = null;
        holdingHealth = 0;
        holdingMagic = 0;
        holdingDeath = false;
        clothingName = null;
    }
}
