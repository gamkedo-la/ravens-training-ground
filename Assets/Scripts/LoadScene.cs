using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public string levelToLoad;
    public bool isFloor1, isFloor2;

    private void Start()
    {
        if (isFloor1 && GameManager.floor2Unlocked)
            this.GetComponent<Button>().interactable = true;

        if (isFloor2 && GameManager.floor3Unlocked)
            this.GetComponent<Button>().interactable = true;
    }

    public void LevelToLoad()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isFloor1)
                GameManager.floor2Unlocked = true;
            if (isFloor2)
                GameManager.floor3Unlocked = true;
            LevelToLoad();
        }
    }
}
