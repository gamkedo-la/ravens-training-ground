using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdateUIForParty : MonoBehaviour
{
    public TMP_Text nameText, descriptor, yes, no;
    public string nameToDisplay, descToDisplay, yesToDisplay, noToDisplay;
    public Button yesButton, noButton;
    public Color color;

    private void Update()
    {
        nameText.text = nameToDisplay;
        descriptor.text = descToDisplay;
        yes.text = yesToDisplay;
        no.text = noToDisplay;
         

        if (GameManager.inCurrentParty.Count >= 4 || GameManager.inCurrentParty.Contains(nameToDisplay))
            yesButton.interactable = false;
        else
            yesButton.interactable = true;
    }

    public void TopButton()
    {
        if (GameManager.inCurrentParty.Count < 4)
        {
            GameManager.inCurrentParty.Add(nameToDisplay);
            GameManager.colorsInParty.Add(color);

            GameManager.charactersNotInParty.Remove(nameToDisplay);
            GameManager.colorsNotInParty.Remove(color);
        }
        this.gameObject.SetActive(false);

        for (int i = 0; i < GameManager.inCurrentParty.Count; i++)
        {
            print(GameManager.inCurrentParty[i]);
            print(GameManager.colorsInParty[i]);
        }
    }

    public void LowerButton()
    {
        if (GameManager.inCurrentParty.Contains(nameToDisplay))
        {
            GameManager.inCurrentParty.Remove(nameToDisplay);
            GameManager.colorsInParty.Remove(color);

            GameManager.charactersNotInParty.Add(nameToDisplay);
            GameManager.colorsNotInParty.Add(color);
        }
        this.gameObject.SetActive(false);
    }
}
