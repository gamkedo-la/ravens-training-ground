using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButtonScript : MonoBehaviour
{
    [SerializeField] GameObject helpMenuCanvas;
    private SubMenuFader subMenuFader;
    [SerializeField] GameObject mainMenuCanvas;
    private CanvasGroup mainMenuCanvasGroupComponent;

    private float fadeSpeed = 1f;
    public bool fadingOutMainMenu = false;

    private void Awake()
    {
        mainMenuCanvasGroupComponent = mainMenuCanvas.GetComponent<CanvasGroup>();
        subMenuFader = helpMenuCanvas.transform.GetComponent<SubMenuFader>();
    }

    private void Update()
    {
        if (fadingOutMainMenu)
        {
            //Debug.Log("inside fading out check");
            FadeOutMainMenu();
        }
    }
    public void HandleClick()
    {
        //Debug.Log("anything");
        StartMainMenuFadeout();
    }

    private void StartMainMenuFadeout()
    {
        fadingOutMainMenu = true;
    }
    private void FadeOutMainMenu()
    {
        mainMenuCanvasGroupComponent.alpha -= fadeSpeed * Time.deltaTime;

        if (mainMenuCanvasGroupComponent.alpha <= 0f)
        {
            mainMenuCanvasGroupComponent.alpha = 0f;
            fadingOutMainMenu = false;
            ToggleHelpAndMainMenus();
        }
    }

    private void ToggleHelpAndMainMenus()
    {
        helpMenuCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);
        subMenuFader.isFadingIn = true;
    }
}
