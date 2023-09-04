using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class IndividualItemLayout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text itemName, itemStock, itemCost;
    public Expendable specificItem;

    public TMP_Text shinyText;
    GameObject gameManager;

    public GameObject descriptorBox;
    public TMP_Text descriptorText;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
        shinyText.text = GameManager.shinyThings.ToString("F0");

        itemName.text = specificItem.nameOfItem;
        itemStock.text = specificItem.ravenStock.ToString("F0");
        itemCost.text = specificItem.Cost.ToString("F0");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptorBox.SetActive(true);
        descriptorText.text = specificItem.whatDoesItDo;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptorBox.SetActive(false);
        descriptorText.text = "";
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
