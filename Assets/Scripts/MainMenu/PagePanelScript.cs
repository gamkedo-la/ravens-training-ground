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
    [SerializeField] TextMeshProUGUI helpTextGameObject;

    [SerializeField] Button backButton;
    [SerializeField] Button forwardButton;

    #endregion
    
    public void HandleBackButtonClick()
    {

    }

    public void HandleForwardButtonClick()
    {

    }
}
