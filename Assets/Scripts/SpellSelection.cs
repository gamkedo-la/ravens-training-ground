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
        battle.currentCombatantUnit.DetermineAbilityFromList(this.gameObject.GetComponent<SpellPrefab>().associatedAttack);
        if (!isAttacking)
        {
            //going from the spell selection screen, then storing it into battle.cs

            battle.TurnOnIndividualAttackItems();

            playerUI.OpenAttackingMenu();
        }
        else
        {
            print($"Player selected spell {battle.selectedSpell.name}");
            //triggering the spell from the unit to perform the above listed spell
            battle.Combatants[battle.currentCombatant].GetComponent<Unit>().TakingUnitTurn(battle.selectedSpell);
            playerUI.TurnOffAllMenus();
            battle.selectedSpell = null;
        }
    }
}
