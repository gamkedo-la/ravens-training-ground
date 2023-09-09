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

    public Magic[] players;
    public GameObject macroUIManager;

    #region useableOutOfBattle
    
    #endregion


    private void Start()
    {
        gameManager = GameObject.Find("GameManager");

        itemName.text = specificItem.name;

        PullPlayerItems();
    }

    void PullPlayerItems()
    {
        itemQuantity.text = specificItem.playerStock.ToString("F0");

        if (specificItem.playerStock <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void UsingItemFirstStep()
    {
        print("here");
        if (!specificItem.solo)
        {
            print("group item, using it now");
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

                print("increase all MP");
                specificItem.playerStock -= 1;
                if (specificItem.playerStock <= 0)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            print("one person got a health or mana up or revive");
            /*for (int i = 0; i < playerButtons.Length; i++)
            {
                playerButtons[i].GetComponent<Button>().interactable = true;
            }*/
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
        previousMenu.SetActive(true);
        currentMenu.SetActive(false);
    }
}
