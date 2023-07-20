using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PagePanelScript : MonoBehaviour
{
    /// <summary>
    /// Define the behavior of the help pages
    /// </summary>
    /// 

    #region cached references

    [SerializeField] List<string> listOfHelpPageStrings = new List<string>();
    [SerializeField] List<Sprite> listOfHelpPageScreenshots = new List<Sprite>();

    [SerializeField] Image screenshotImageGameObject;
    private Image actualImageComponent;
    [SerializeField] TextMeshProUGUI helpTextGameObject;

    [SerializeField] Button backButton;
    [SerializeField] Button forwardButton;

    private int numberOfScreenshots;
    private int currentListOfScreenShotsIndex = 0;

    #endregion

    private void Start()
    {
        numberOfScreenshots = listOfHelpPageScreenshots.Count;
    }

    public void HandleBackButtonClick()
    {
        currentListOfScreenShotsIndex -= 1;
        if (currentListOfScreenShotsIndex < 0)
        {
            currentListOfScreenShotsIndex = 0;
        }

        screenshotImageGameObject.sprite = listOfHelpPageScreenshots[currentListOfScreenShotsIndex];
        helpTextGameObject.text = listOfHelpPageStrings[currentListOfScreenShotsIndex];
    }

    public void HandleForwardButtonClick()
    {
        currentListOfScreenShotsIndex += 1;
        if (currentListOfScreenShotsIndex > numberOfScreenshots - 1)
        {
            currentListOfScreenShotsIndex = numberOfScreenshots - 1;
        }

        screenshotImageGameObject.sprite = listOfHelpPageScreenshots[currentListOfScreenShotsIndex];
        helpTextGameObject.text = listOfHelpPageStrings[currentListOfScreenShotsIndex];
    }
}
