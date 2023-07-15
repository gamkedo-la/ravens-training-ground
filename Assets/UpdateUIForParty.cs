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

    private void Update()
    {
        nameText.text = nameToDisplay;
        descriptor.text = descToDisplay;
        yes.text = yesToDisplay;
        no.text = noToDisplay;

        if (GameManager.inCurrentParty.Count >= 4)
            yesButton.interactable = false;
    }
}
