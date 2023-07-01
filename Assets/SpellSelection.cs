using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellSelection : MonoBehaviour
{
    Battle battle;
    BattleMenu playerUI;

    public bool isAttacking;

    private void Start()
    {
        battle = GameObject.Find("Battle").GetComponent<Battle>();
        playerUI = GameObject.Find("PlayerUICanvas").GetComponent<BattleMenu>();
    }
    public void SpellSelected()
    {
        if (!isAttacking)
        {
            //going from the spell selection screen, then storing it into battle.cs
            battle.spellToStore = this.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;

            playerUI.OpenAttackingMenu();
        }
        else
        {
            print(battle.spellToStore);
            //triggering the spell from the unit to perform the above listed spell
            battle.Combatants[battle.currentCombatant].GetComponent<Unit>().TakingUnitTurn();
            playerUI.TurnOffAllMenus();
            battle.spellToStore = "";
        }
    }
}
