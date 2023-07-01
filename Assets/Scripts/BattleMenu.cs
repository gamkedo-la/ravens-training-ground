using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    public GameObject firstMenu, tacticsMenu, fleeMenu, guardMenu, spellsMenu, itemsMenu, attackingMenu;
    public GameObject tacticsMenuForLeader;

    Battle battle;
    public GameObject spellsUIPrefab;
    public Transform spellsMenuHolder;
    public bool isSpellBackButton;

    public List<Sprite> icons = new List<Sprite>();

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
        attackingMenu.SetActive(false);

        PopulateSpellsMenu();
    }

    public void OpenAttackingMenu()
    {
        //store string for attack

        battle.TurnOnIndividualAttackItems();

        //turn off all menus
        TurnOffAllMenus();
        //turn on attacking menu
        attackingMenu.SetActive(true);
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
        attackingMenu.SetActive(false);
        //if cam position changed or particles are turn on, this resets it
        battle.TurnOffIndividualAttackItems();
        if (isSpellBackButton)
            ClearStoredSpell();
    }

    void ClearStoredSpell()
    {
        battle.spellToStore = "";
    }

    public void PopulateSpellsMenu()
    {
        //Destroy all child count before reloading this menu (that way it doesn't duplicate it)

        for (int i = 0; i < battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks.Count - 1; i++)
        {
            GameObject instantiatedPrefab = Instantiate(spellsUIPrefab, spellsMenuHolder.transform.position, spellsMenuHolder.transform.rotation) as GameObject;
            instantiatedPrefab.transform.parent = spellsMenuHolder.transform;
            instantiatedPrefab.transform.localScale = new Vector3(5, 0.4f, 1);

            instantiatedPrefab.transform.Find("SpellName").GetComponent<TextMeshProUGUI>().text = battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks[i].AttackName;
            instantiatedPrefab.transform.Find("SpellDesc").GetComponent<TextMeshProUGUI>().text = battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks[i].AttackDescription;
            instantiatedPrefab.transform.Find("SpellCost").GetComponent<TextMeshProUGUI>().text = battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks[i].cost.ToString();

            if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks[i].effectType.ToString() == "Ancient")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[0];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks[i].effectType.ToString() == "DarkArts")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[1];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks[i].effectType.ToString() == "Charms")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[2];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks[i].effectType.ToString() == "Transfiguration")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[3];
            //    instantiatedPrefab.GetComponent<Button>().highlightedColor = new Color(238, 23, 33);
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks[i].effectType.ToString() == "Physical")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[4];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "HP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().attacks[i].effectType.ToString() == "Potions")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[5];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
        }
    }
}
