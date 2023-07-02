using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyDifficultyButtonScript : MonoBehaviour
{
    [SerializeField] Image easyButtonBackgroundImage;
    [SerializeField] Image mediumButtonBackgroundImage;
    [SerializeField] Image hardButtonBackgroundImage;

    [SerializeField] Image easyButtonImage;
    [SerializeField] Image mediumButtonImage;
    [SerializeField] Image hardButtonImage;

    public Color unhighlightedColorSwatch;
    public Color highlightedColorSwatch;

    public Color buttonColorSwatch;

    public void HandleClick()
    {
        easyButtonBackgroundImage.color = highlightedColorSwatch;
        mediumButtonBackgroundImage.color = unhighlightedColorSwatch;
        hardButtonBackgroundImage.color = unhighlightedColorSwatch;

        easyButtonImage.color = buttonColorSwatch;
        mediumButtonImage.color = buttonColorSwatch;
        hardButtonImage.color = buttonColorSwatch;


        PlayerPrefs.SetString("GameDifficulty", "Easy");

        Debug.Log(PlayerPrefs.GetString("GameDifficulty"));
    }
}
