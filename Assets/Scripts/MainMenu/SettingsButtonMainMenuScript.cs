using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtonMainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject settingsCanvas;
    private SubMenuFader subMenuFader;
    [SerializeField] GameObject mainMenuCanvas;
    private CanvasGroup mainMenuCanvasGroupComponent;

    private float fadeSpeed = 1f;
    public bool fadingOutMainMenu = false;

    private void Awake()
    {
        mainMenuCanvasGroupComponent = mainMenuCanvas.GetComponent<CanvasGroup>();
        subMenuFader = settingsCanvas.transform.GetComponent<SubMenuFader>();
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
            ToggleSettingsAndMainMenus();
        }    
    }    

    private void ToggleSettingsAndMainMenus()
    {
        settingsCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);
        subMenuFader.isFadingIn = true;
    }    
}
