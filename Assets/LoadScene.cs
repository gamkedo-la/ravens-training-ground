using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public string levelToLoad;
    public bool isFloor2, isFloor3;

    private void Start()
    {
        if (isFloor2 && GameManager.floor2Unlocked)
            this.GetComponent<Button>().interactable = true;

        if (isFloor3 && GameManager.floor3Unlocked)
            this.GetComponent<Button>().interactable = true;
    }

    public void LevelToLoad()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
