using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonMainMenuScript : MonoBehaviour
{
    public void HandleClick()
    {
        Debug.Log("quit only works on build");
        Application.Quit();
    }
}
