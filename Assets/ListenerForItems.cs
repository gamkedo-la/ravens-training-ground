using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Stats;
using UnityEngine.UI;

public class ListenerForItems : MonoBehaviour
{
    Expendable specificItem;
    public Health health;
    public Magic magic;

    public GameObject macroUIHolder;
    public Button[] allPlayerButtons;

    public void ConsumingItem()
    {
        print("here");
        if (specificItem.nameOfItem == GameManager.itemName)
            specificItem.playerStock -= 1;

        if (specificItem.reincarnate && health.HitPoints >= 0)
            this.GetComponent<Button>().interactable = false;

        else
            this.GetComponent<Button>().interactable = true;

        health.HitPoints += GameManager.holdingHealth;
        magic.MagicPoints += GameManager.holdingMagic;
        health.IsDead();

        if (health.HitPoints >= health.startingHitPoints)
            health.HitPoints = health.startingHitPoints;

        if (magic.MagicPoints >= magic.startingMagicPoints)
            magic.MagicPoints = magic.startingMagicPoints;

        macroUIHolder.GetComponent<UpdatePlayerStatsUI>().UpdatePlayerUI();

        GameManager.ClearGameManagerDataFromInventory();

        for (int i = 0; i < allPlayerButtons.Length; i++)
        {
            allPlayerButtons[i].interactable = false;
        }
        print("here");
    }
}
