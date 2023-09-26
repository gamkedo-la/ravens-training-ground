using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonSettingsMenuScript : MonoBehaviour
{
    [SerializeField] GameObject subMenu;
    private SubMenuFader subMenuScript;
    [SerializeField] GameObject mainMenu;

    private void Awake()
    {
        subMenuScript = subMenu.gameObject.transform.GetComponent<SubMenuFader>();
    }
    
    public void HandleClick()
    {
        subMenuScript.isFadingOut = true;
    }    

    private void ToggleSettingsAndMainMenus()
    {
        mainMenu.gameObject.SetActive(true);
        subMenu.gameObject.SetActive(false);
    }     
}
