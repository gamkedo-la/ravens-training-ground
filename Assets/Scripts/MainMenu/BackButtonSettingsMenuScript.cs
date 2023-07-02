using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonSettingsMenuScript : MonoBehaviour
{
    [SerializeField] GameObject SettingsCanvas;
    private SettingsCanvasScript settingsCanvasScript;
    [SerializeField] GameObject MainMenuCanvas;

    private void Awake()
    {
        settingsCanvasScript = SettingsCanvas.gameObject.transform.GetComponent<SettingsCanvasScript>();
    }
    public void HandleClick()
    {
        settingsCanvasScript.isFadingOut = true;
    }    

    private void ToggleSettingsAndMainMenus()
    {
        MainMenuCanvas.gameObject.SetActive(true);
        SettingsCanvas.gameObject.SetActive(false);
    }     
}
