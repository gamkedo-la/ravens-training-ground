using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    public GameObject firstMenu, tacticsMenu, fleeMenu, guardMenu, spellsMenu, itemsMenu;
    public GameObject tacticsMenuForLeader;

    Battle battle;

    //this is starting at every player turn, Tactics menu should be turned off for all but Dani.
    //Any 'Back' button goes to 'OpenFirstMenu'
    public void OpenFirstMenu()
    {
        /*if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().Name == "Dani")
            tacticsMenuForLeader.SetActive(true);
        else
            tacticsMenuForLeader.SetActive(false);
        */
        firstMenu.SetActive(true);
        tacticsMenu.SetActive(false);
        fleeMenu.SetActive(false);
        guardMenu.SetActive(false);
        spellsMenu.SetActive(false);
        itemsMenu.SetActive(false);
    }

    public void OpenTacticsMenu()
    {
        firstMenu.SetActive(false);
        tacticsMenu.SetActive(true);
        fleeMenu.SetActive(false);
        guardMenu.SetActive(false);
        spellsMenu.SetActive(false);
        itemsMenu.SetActive(false);
    }

    public void OpenFleeMenu()
    {
        fleeMenu.SetActive(true);
    }

    public void OpenGuardMenu()
    {
        guardMenu.SetActive(true);
    }

    public void OpenSpellsMenu()
    {
        firstMenu.SetActive(false);
        tacticsMenu.SetActive(false);
        fleeMenu.SetActive(false);
        guardMenu.SetActive(false);
        spellsMenu.SetActive(true);
        itemsMenu.SetActive(false);
    }

    public void OpenItemsMenu()
    {
        firstMenu.SetActive(false);
        tacticsMenu.SetActive(false);
        fleeMenu.SetActive(false);
        guardMenu.SetActive(false);
        spellsMenu.SetActive(false);
        itemsMenu.SetActive(true);
    }
}
