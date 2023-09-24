using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dungeonChest : MonoBehaviour
{
    bool hasEntered;
    Animator anim;

    GameObject gameManager;

    public int shinyMin, shinyCount, shinyMax;

    public TMP_Text shinyText, itemText;

    int randomlySelect;
    string split;
    public Expendable[] InventoryPool;

    float countDownTime = 3;
    bool beginCountdown;
    private void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");
    }
    private void Update()
    {
        if (hasEntered && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("hasOpened", true);

            SelectInvItem();
            if (gameManager.GetComponent<GameManager>().playerInventory.Contains(InventoryPool[randomlySelect]))
            {
                InventoryPool[randomlySelect].playerStock += 1;
            }

            else
                gameManager.GetComponent<GameManager>().playerInventory.Add(InventoryPool[randomlySelect]);

            shinyText.text = shinyCount.ToString();
            split = InventoryPool[randomlySelect].ToString();

            split = split.Replace("(Expendable)", "");

            itemText.text = split;

            countDownTime = 3;
            beginCountdown = true;
        }

        if (beginCountdown)
        {
            countDownTime -= Time.deltaTime;
            if (countDownTime <= 0)
            {
                shinyText.text = "";
                itemText.text = "";
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hasEntered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hasEntered = false;
        }
    }

    public void SelectInvItem()
    {
        randomlySelect = Random.Range(0, InventoryPool.Length);
        shinyCount = Random.Range(shinyMin, shinyMax);

        GameManager.shinyThings += shinyCount;
    }
}
