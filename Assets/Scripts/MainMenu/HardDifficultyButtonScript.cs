using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HardDifficultyButtonScript : MonoBehaviour
{
    [SerializeField] Image easyButtonBackgroundImage;
    [SerializeField] Image mediumButtonBackgroundImage;
    [SerializeField] Image hardButtonBackgroundImage;

    public Color unhighlightedColorSwatch;
    public Color highlightedColorSwatch;

    [SerializeField] Image easyButtonImage;
    [SerializeField] Image mediumButtonImage;
    [SerializeField] Image hardButtonImage;

    public Color buttonColorSwatch;

    public void HandleClick()
    {
        easyButtonBackgroundImage.color = unhighlightedColorSwatch;
        mediumButtonBackgroundImage.color = unhighlightedColorSwatch;
        hardButtonBackgroundImage.color = highlightedColorSwatch;

        easyButtonImage.color = buttonColorSwatch;
        mediumButtonImage.color = buttonColorSwatch;
        hardButtonImage.color = buttonColorSwatch;

        PlayerPrefs.SetString("GameDifficulty", "Hard");

        Debug.Log(PlayerPrefs.GetString("GameDifficulty"));
    }
}
