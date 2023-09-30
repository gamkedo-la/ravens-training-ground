using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroDialog : MonoBehaviour
{
    public string[] names, messages;
    int currentCount;

    public TMP_Text nameText, messageText;
    public GameObject turnCanvasOn;
    public bool isEndScene;

    public Image[] toToggleOff;
    public TMP_Text[] toToggleOffText;

    public GameObject[] deathParticle, players;

    public bool isIntro;
    public static bool hasSeenIntro;

    private void Start()
    {
        nameText.text = names[0].ToString();
        messageText.text = messages[0].ToString();

        if (isIntro && hasSeenIntro)
        {
            this.gameObject.SetActive(false);
        }
        if (isIntro && !hasSeenIntro)
        {
            for (int i = 0; i < toToggleOff.Length; i++)
            {
                toToggleOff[i].GetComponent<Image>().enabled = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Floor0");
            print("reload");
        }
    }
    public void NextButton()
    {
        currentCount++;

        if (currentCount > 11 && !isEndScene)
        {
            SceneManager.LoadScene("Floor0");
        }
        else
        {
            if (currentCount > 7 && isEndScene)
            {
                for (int i = 0; i < toToggleOff.Length; i++)
                {
                    toToggleOff[i].GetComponent<Image>().enabled = false;
                }

                for (int i = 0; i < toToggleOffText.Length; i++)
                {
                    toToggleOffText[i].text = "";
                }
                StartCoroutine(Return());
            }
            if (isIntro && currentCount >= names.Length)
            {
                for (int i = 0; i < toToggleOff.Length; i++)
                {
                    toToggleOff[i].GetComponent<Image>().enabled = false;
                }

                for (int i = 0; i < toToggleOffText.Length; i++)
                {
                    toToggleOffText[i].text = "";
                }
                hasSeenIntro = true;
                print(hasSeenIntro);
            }

            nameText.text = names[currentCount].ToString();
            messageText.text = messages[currentCount].ToString();
        }
    }

    public void TurnObjectOn()
    {
        if(!turnCanvasOn.activeSelf)
            turnCanvasOn.SetActive(true);
    }

    IEnumerator Return()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < deathParticle.Length; i++)
        {
            deathParticle[i].SetActive(true);
        }
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
        }
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("MainMenu");
    }
}
