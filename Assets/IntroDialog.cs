using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroDialog : MonoBehaviour
{
    public string[] names, messages;
    int currentCount;

    public TMP_Text nameText, messageText;
    public GameObject turnCanvasOn;

    private void Start()
    {
        nameText.text = names[0].ToString();
        messageText.text = messages[0].ToString();
    }
    public void NextButton()
    {
        currentCount++;

        if (currentCount > 11)
        {
            SceneManager.LoadScene("Floor0");
        }
        else
        {
            nameText.text = names[currentCount].ToString();
            messageText.text = messages[currentCount].ToString();
        }
    }

    public void TurnObjectOn()
    {
        turnCanvasOn.SetActive(true);
    }
}
