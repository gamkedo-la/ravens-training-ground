using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PullPlayersClothing : MonoBehaviour
{
    public GameObject[] wands, maleShirt, malePants, maleHat, femaleShirt, femalePants, femaleHat;
    public Clothing specificClothing;

    TMP_Text itemName, stock;

    public GameObject wandPage, hatPage, shirtPage, pantsPage;

    private void Start()
    {
        itemName = gameObject.transform.GetChild(2).GetComponent<TMP_Text>();
        stock = gameObject.transform.GetChild(1).GetComponent<TMP_Text>();

        itemName.text = specificClothing.nameOfItem;
        stock.text = specificClothing.playerStock.ToString("F0");
    }

    public void ToggleWands()
    {
        GameManager.ClearGameManagerDataFromInventory();
        TurnAllOff();

        wandPage.SetActive(true);
        hatPage.SetActive(false);
        shirtPage.SetActive(false);
        pantsPage.SetActive(false);
    }

    public void ToggleHats()
    {
        GameManager.ClearGameManagerDataFromInventory();
        TurnAllOff();

        wandPage.SetActive(false);
        hatPage.SetActive(true);
        shirtPage.SetActive(false);
        pantsPage.SetActive(false);
    }

    public void ToggleShirts()
    {
        GameManager.ClearGameManagerDataFromInventory();
        TurnAllOff();

        wandPage.SetActive(false);
        hatPage.SetActive(false);
        shirtPage.SetActive(true);
        pantsPage.SetActive(false);
    }

    public void TogglePants()
    {
        GameManager.ClearGameManagerDataFromInventory();
        TurnAllOff();

        wandPage.SetActive(false);
        hatPage.SetActive(false);
        shirtPage.SetActive(false);
        pantsPage.SetActive(true);
    }

    public void TurnSlotsOn()
    {
        TurnAllOff();
        #region Hat
        if (specificClothing.equipableSlot == "head" && specificClothing.gender == "male")        {
            for (int i = 0; i < maleHat.Length; i++)
                maleHat[i].SetActive(true);
        }
        else if (specificClothing.equipableSlot == "head" && specificClothing.gender == "female")
        {
            for (int i = 0; i < femaleHat.Length; i++)
                femaleHat[i].SetActive(true);
        }
        else if (specificClothing.equipableSlot == "head" && specificClothing.gender == "unisex")
        {
            for (int i = 0; i < femaleHat.Length; i++)
                femaleHat[i].SetActive(true);

            for (int i = 0; i < maleHat.Length; i++)
                maleHat[i].SetActive(true);
        }
        #endregion

        #region Wand
        else if (specificClothing.equipableSlot == "hand")
        {
            for (int i = 0; i < wands.Length; i++)
                wands[i].SetActive(true);
        }
        #endregion

        #region Shirts
        if (specificClothing.equipableSlot == "chest" && specificClothing.gender == "male")
        {
            for (int i = 0; i < maleShirt.Length; i++)
                maleShirt[i].SetActive(true);
        }
        else if (specificClothing.equipableSlot == "chest" && specificClothing.gender == "female")
        {
            for (int i = 0; i < femaleShirt.Length; i++)
                femaleShirt[i].SetActive(true);
        }
        else if (specificClothing.equipableSlot == "chest" && specificClothing.gender == "unisex")
        {
            for (int i = 0; i < maleShirt.Length; i++)
                maleShirt[i].SetActive(true);

            for (int i = 0; i < femaleShirt.Length; i++)
                femaleShirt[i].SetActive(true);
        }
        #endregion

        #region Pants
        if (specificClothing.equipableSlot == "legs" && specificClothing.gender == "male")
        {
            for (int i = 0; i < malePants.Length; i++)
                malePants[i].SetActive(true);
        }
        else if (specificClothing.equipableSlot == "legs" && specificClothing.gender == "female")
        {
            for (int i = 0; i < malePants.Length; i++)
                femalePants[i].SetActive(true);
        }
        #endregion
        GameManager.clothingName = specificClothing;
    }

    void TurnAllOff()
    {
        #region Turn All of them off for leftover data
        for (int i = 0; i < wands.Length; i++)
            wands[i].SetActive(false);

        for (int i = 0; i < maleShirt.Length; i++)
            maleShirt[i].SetActive(false);

        for (int i = 0; i < femaleShirt.Length; i++)
            femaleShirt[i].SetActive(false);

        for (int i = 0; i < malePants.Length; i++)
            malePants[i].SetActive(false);

        for (int i = 0; i < femalePants.Length; i++)
            femalePants[i].SetActive(false);

        for (int i = 0; i < maleHat.Length; i++)
            maleHat[i].SetActive(false);

        for (int i = 0; i < femaleHat.Length; i++)
            femaleHat[i].SetActive(false);
        #endregion
    }
}
