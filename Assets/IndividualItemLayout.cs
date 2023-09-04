using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndividualItemLayout : MonoBehaviour
{
    public TMP_Text itemName, itemStock, itemCost;
    public Expendable specificItem;

    public TMP_Text shinyText;
    GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
        shinyText.text = GameManager.shinyThings.ToString("F0");

        itemName.text = specificItem.nameOfItem;
        itemStock.text = specificItem.ravenStock.ToString("F0");
        itemCost.text = specificItem.Cost.ToString("F0");
    }

    public void PurchaseItem()
    {
        print("currentGameManagerbalance " + GameManager.shinyThings);
        shinyText.text = GameManager.shinyThings.ToString("F0");

        if (GameManager.shinyThings >= specificItem.Cost)
        {
            GameManager.shinyThings -= specificItem.Cost;
            print(GameManager.shinyThings);
            shinyText.text = GameManager.shinyThings.ToString("F0");

            if (gameManager.GetComponent<GameManager>().playerInventory.Contains(specificItem))
            {
                specificItem.playerStock += 1;
            }

            else
                gameManager.GetComponent<GameManager>().playerInventory.Add(specificItem);

            specificItem.ravenStock -= 1;
            if (specificItem.ravenStock <= 0)
            {
                Destroy(this.gameObject);
            }

            itemName.text = specificItem.nameOfItem;
            itemStock.text = specificItem.ravenStock.ToString("F0");
            itemCost.text = specificItem.Cost.ToString("F0");
        }

    }

    // on hover, show description
}
