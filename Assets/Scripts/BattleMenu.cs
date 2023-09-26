using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    public GameObject firstMenu, tacticsMenu, fleeMenu, guardMenu, spellsMenu, itemsMenu, attackingMenu, groupAttackMenu;
    public GameObject tacticsMenuForLeader;

    Battle battle;
    public GameObject spellsUIPrefab;
    public Transform spellsMenuHolder;
    public bool isSpellBackButton;

    public List<Sprite> icons = new List<Sprite>();

    public delegate void AttackMenuOpened(bool open);
    public event AttackMenuOpened AttackMenuOpenedEvent;

    private void Start()
    {
        battle = GameObject.Find("Battle").GetComponent<Battle>();
    }

    private void Update()
    {
       // this is throwing a ton of errors, commenting out for now
        /*
        if (battle.groupAttackHappening)
        {
            TurnOffAllMenus();
            groupAttackMenu.SetActive(true);
        }*/

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
        TurnOffAllMenus();

        battle.MoveCamera();

        firstMenu.SetActive(true);
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

        battle.ShiftCamera();

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
        //turn off all menus
        TurnOffAllMenus();
        
        AttackMenuOpenedEvent(true);

        //store string for attack

        battle.TurnOnIndividualAttackItems();


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

    public void AttackAll()
    {
       // if (battle.groupAttackHappening)
            print("group attack happening");
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
        groupAttackMenu.SetActive(false);
        //if cam position changed or particles are turn on, this resets it
        battle.TurnOffIndividualAttackItems();
        if (isSpellBackButton)
            ClearStoredSpell();

        AttackMenuOpenedEvent(false);
    }

    void ClearStoredSpell()
    {
        battle.selectedSpell = null;
        battle.TurnOffIndividualAttackItems();
    }

    public void PopulateSpellsMenu()
    {
        //Destroy all child count before reloading this menu (that way it doesn't duplicate it)

        for (int i = 0; i < battle.Combatants[battle.currentCombatant].GetComponent<Unit>().abilities.Count - 1; i++)
        {
            GameObject instantiatedPrefab = Instantiate(spellsUIPrefab, spellsMenuHolder.transform);
            //instantiatedPrefab.transform.parent = spellsMenuHolder.transform;
            //instantiatedPrefab.transform.localScale = new Vector3(5, 0.4f, 1);
            AbilityBase associatedAttack = battle.Combatants[battle.currentCombatant].GetComponent<Unit>().abilities[i];
            instantiatedPrefab.transform.Find("SpellName").GetComponent<TextMeshProUGUI>().text = associatedAttack.AttackName;
            instantiatedPrefab.transform.Find("SpellDesc").GetComponent<TextMeshProUGUI>().text = associatedAttack.AttackDescription;
            int displayCost = (int)associatedAttack.cost;
            instantiatedPrefab.transform.Find("SpellCost").GetComponent<TextMeshProUGUI>().text = displayCost.ToString();
            instantiatedPrefab.GetComponent<SpellPrefab>().associatedAttack = associatedAttack;


            if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().abilities[i].effectType.ToString() == "Ancient")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[0];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().abilities[i].effectType.ToString() == "DarkArts")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[1];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().abilities[i].effectType.ToString() == "Charms")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[2];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().abilities[i].effectType.ToString() == "Transfiguration")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[3];
            //    instantiatedPrefab.GetComponent<Button>().highlightedColor = new Color(238, 23, 33);
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().abilities[i].effectType.ToString() == "Physical")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[4];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "HP";
            }
            else if (battle.Combatants[battle.currentCombatant].GetComponent<Unit>().abilities[i].effectType.ToString() == "Potions")
            {
                instantiatedPrefab.transform.Find("SpellIcon").GetComponent<Image>().sprite = icons[5];
                instantiatedPrefab.transform.Find("MPHP").GetComponent<TextMeshProUGUI>().text = "MP";
            }
        }
    }
}
