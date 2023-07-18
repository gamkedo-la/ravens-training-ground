using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonSettingsMenuScript : MonoBehaviour
{
    [SerializeField] GameObject HelpMenuCanvas;
    private HelpCanvasScript helpMenuCanvasScript;
    [SerializeField] GameObject MainMenuCanvas;

    private void Awake()
    {
        helpMenuCanvasScript = HelpMenuCanvas.gameObject.transform.GetComponent<HelpCanvasScript>();
    }
    public void HandleClick()
    {
        helpMenuCanvasScript.isFadingOut = true;
    }    

    private void ToggleSettingsAndMainMenus()
    {
        MainMenuCanvas.gameObject.SetActive(true);
        HelpMenuCanvas.gameObject.SetActive(false);
    }     
}
