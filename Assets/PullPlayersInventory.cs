using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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


    private void Start()
    {
        gameManager = GameObject.Find("GameManager");

        itemName.text = specificItem.name;

        PullPlayerItems();
    }

    void PullPlayerItems()
    {
        if (specificItem.onlyUsedInBattle)
            this.gameObject.SetActive(false);
        else
        {
            if (gameManager.GetComponent<GameManager>().playerInventory.Contains(specificItem))
            {
                itemQuantity.text = specificItem.playerStock.ToString("F0");
            }
            else
                this.gameObject.SetActive(false);
        }
    }

    public void UsingItemFirstStep()
    {
        if (!specificItem.solo)
        {
            print("group item, using it now");
            if (specificItem.magicPoints)
            {
                print("increase all MP");
                specificItem.playerStock -= 1;
            }
        }
        else
        {
            print("one person got a health or mana up");
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
