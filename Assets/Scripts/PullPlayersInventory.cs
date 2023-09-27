using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Character.Stats;
using System.Linq;

public class PullPlayersInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject gameManager;
    public GameObject descriptorBox;
    public TMP_Text descriptorText;

    public Expendable specificItem;

    public TMP_Text itemName, itemQuantity;

    public GameObject[] playerButtons;

    public bool isBackButton;
    public GameObject currentMenu, previousMenu;

    public Health[] playerHealth;
    public Magic[] players;
    public GameObject macroUIManager;

    public Button[] playerUI;

    #region useableOutOfBattle
    
    #endregion


    private void Start()
    {
        gameManager = GameObject.Find("GameManager");

        itemName.text = specificItem.name;

        PullPlayerItems();
    }

    public void PullPlayerItems()
    {
        itemQuantity.text = specificItem.playerStock.ToString("F0");

        if (specificItem.playerStock <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void UsingItemFirstStep()
    {
        GameManager.ClearGameManagerDataFromInventory();
        if (!specificItem.solo)
        {
            if (specificItem.magicPoints)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].MagicPoints += 50;
                    if (players[i].MagicPoints >= players[i].startingMagicPoints)
                    {
                        players[i].MagicPoints = players[i].startingMagicPoints;
                    }
                }

                itemQuantity.text = specificItem.playerStock.ToString("F0");
                macroUIManager.GetComponent<UpdatePlayerStatsUI>().UpdatePlayerUI();

                specificItem.playerStock -= 1;
                if (specificItem.playerStock <= 0)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < playerUI.Length; i++)
            {
                if (specificItem.magicPoints)
                    playerUI[i].interactable = true;

                else if (playerHealth[i].HitPoints <= 0 && specificItem.reincarnate)
                    playerUI[i].interactable = true;
                else if (playerHealth[i].HitPoints > 0 && !specificItem.reincarnate)
                    playerUI[i].interactable = true;
            }

            GameManager.itemName = specificItem;
            if (specificItem.health)
            {
                GameManager.holdingHealth = specificItem.specificAmount;
                GameManager.holdingDeath = false;
            }
            if (specificItem.magicPoints)
                GameManager.holdingMagic = specificItem.specificAmount;
        }


        if (specificItem.playerStock <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isBackButton)
        {
            descriptorBox.SetActive(true);
            descriptorText.text = specificItem.whatDoesItDo;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isBackButton)
        {
            descriptorBox.SetActive(false);
            descriptorText.text = "";
        }
    }

    public void isBack()
    {
        GameManager.ClearGameManagerDataFromInventory();
        previousMenu.SetActive(true);
        currentMenu.SetActive(false);
    }
}
