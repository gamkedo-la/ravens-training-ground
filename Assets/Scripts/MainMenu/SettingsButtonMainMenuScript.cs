using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtonMainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject SettingsCanvas;
    private SettingsCanvasScript settingsCanvasScript;
    [SerializeField] GameObject MainMenuCanvas;
    private CanvasGroup mainMenuCanvasGroupComponent;

    private float fadeSpeed = 1f;
    public bool fadingOutMainMenu = false;

    private void Awake()
    {
        mainMenuCanvasGroupComponent = MainMenuCanvas.GetComponent<CanvasGroup>();
        settingsCanvasScript = SettingsCanvas.transform.GetComponent<SettingsCanvasScript>();
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
        SettingsCanvas.gameObject.SetActive(true);
        MainMenuCanvas.gameObject.SetActive(false);
        settingsCanvasScript.isFadingIn = true;
    }    
}
