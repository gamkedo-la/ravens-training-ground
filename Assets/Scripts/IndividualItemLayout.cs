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
    public Clothing specificClothes;

    public TMP_Text shinyText;
    GameObject gameManager;

    public GameObject descriptorBox;
    public TMP_Text descriptorText;

    public bool isClothes;
    public TMP_Text type, gender;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");

        if (isClothes)
        {
            gender.text = specificClothes.gender;
            type.text = specificClothes.equipableSlot;

            if (specificClothes.gender == "male")
                gender.color = Color.blue;
            else if (specificClothes.gender == "female")
                gender.color = Color.magenta;
            else
                gender.color = Color.white;

            if (type.text == "hand")
                type.color = Color.gray;
            else if (type.text == "chest")
                type.color = Color.green;
            else if (type.text == "legs")
                type.color = Color.cyan;
            else
                type.color = Color.yellow;

            itemName.text = specificClothes.nameOfItem;
            itemStock.text = specificClothes.ravenStock.ToString("F0");
            itemCost.text = specificClothes.Cost.ToString("F0");
        }

        else
        {
            itemName.text = specificItem.nameOfItem;
            itemStock.text = specificItem.ravenStock.ToString("F0");
            itemCost.text = specificItem.Cost.ToString("F0");
        }

        shinyText.text = GameManager.shinyThings.ToString("F0");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptorBox.SetActive(true);
        if (isClothes)
            descriptorText.text = specificClothes.whatDoesItDo;
        else
             descriptorText.text = specificItem.whatDoesItDo;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptorBox.SetActive(false);
        descriptorText.text = "";
    }

    public void PurchaseItem()
    {
        shinyText.text = GameManager.shinyThings.ToString("F0");

        if (!isClothes && GameManager.shinyThings >= specificItem.Cost)
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

        if(isClothes && GameManager.shinyThings >= specificClothes.Cost)
        {
            GameManager.shinyThings -= specificClothes.Cost;
            print(GameManager.shinyThings);
            shinyText.text = GameManager.shinyThings.ToString("F0");

            if (gameManager.GetComponent<GameManager>().playerClothes.Contains(specificClothes))
            {
                specificClothes.playerStock += 1;
            }

            else
                gameManager.GetComponent<GameManager>().playerClothes.Add(specificClothes);

            specificClothes.ravenStock -= 1;
            if (specificClothes.ravenStock <= 0)
            {
                Destroy(this.gameObject);
            }

            itemName.text = specificClothes.nameOfItem;
            itemStock.text = specificClothes.ravenStock.ToString("F0");
            itemCost.text = specificClothes.Cost.ToString("F0");
        }
    }
}
