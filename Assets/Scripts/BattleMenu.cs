using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    public GameObject firstMenu, tacticsMenu, fleeMenu, guardMenu, spellsMenu, itemsMenu;
    public GameObject tacticsMenuForLeader;

    Battle battle;
    public GameObject spellsUIPrefab;
    public Transform spellsMenuHolder;

    private void Start()
    {
        battle = GameObject.Find("Battle").GetComponent<Battle>();
    }

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

        PopulateSpellsMenu();
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

    public void FleeYes()
    {
        TurnOffAllMenus();
        battle.FleeChance();
    }

    public void GuardYes()
    {
        TurnOffAllMenus();
        battle.GuardPlayer();
    }

    public void TurnOffAllMenus()
    {
        firstMenu.SetActive(false);
        tacticsMenu.SetActive(false);
        fleeMenu.SetActive(false);
        guardMenu.SetActive(false);
        spellsMenu.SetActive(false);
        itemsMenu.SetActive(false);
    }

    public void PopulateSpellsMenu()
    {
        //Destroy all child count before reloading this menu (that way it doesn't duplicate it)

        Debug.Log(transform.childCount);


        for (int i = 0; i < battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks.Count - 1; i++)
        {
            GameObject instantiatedPrefab = Instantiate(spellsUIPrefab, spellsMenuHolder.transform.position, spellsMenuHolder.transform.rotation) as GameObject;
            instantiatedPrefab.transform.parent = spellsMenuHolder.transform;
            instantiatedPrefab.transform.localScale = new Vector3(5, 0.4f, 1);
        }
    }
}
