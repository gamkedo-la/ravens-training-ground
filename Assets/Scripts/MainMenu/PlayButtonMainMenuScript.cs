using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonMainMenuScript : MonoBehaviour
{
    public void HandleClick()
    {
        SceneManager.LoadScene("Floor1");
    }
}
