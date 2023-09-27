using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenus : MonoBehaviour
{
    public GameObject futureMenu, currentMenu;

    public void ChangeMenu()
    {
        futureMenu.SetActive(true);
        currentMenu.SetActive(false);
    }
}
