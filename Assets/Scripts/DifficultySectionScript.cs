using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySectionScript : MonoBehaviour
{
    [SerializeField] Image easyButtonBackgroundImage;
    [SerializeField] Image mediumButtonBackgroundImage;
    [SerializeField] Image hardButtonBackgroundImage;

    public Color unhighlightedColorSwatch;
    public Color highlightedColorSwatch;

    void Start()
    {
        
    }

    public void InitializeGameDifficulty(string gameDifficulty)
    {
        if (gameDifficulty == "Easy")
        {
            easyButtonBackgroundImage.color = highlightedColorSwatch;
            mediumButtonBackgroundImage.color = unhighlightedColorSwatch;
            hardButtonBackgroundImage.color = unhighlightedColorSwatch;
        }    
        else if (gameDifficulty == "Medium")
        {
            easyButtonBackgroundImage.color = unhighlightedColorSwatch;
            mediumButtonBackgroundImage.color = highlightedColorSwatch;
            hardButtonBackgroundImage.color = unhighlightedColorSwatch;
        }    
        else if (gameDifficulty == "Hard")
        {
            easyButtonBackgroundImage.color = unhighlightedColorSwatch;
            mediumButtonBackgroundImage.color = unhighlightedColorSwatch;
            hardButtonBackgroundImage.color = highlightedColorSwatch;
        }    
    }    
}
