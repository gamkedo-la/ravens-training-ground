using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RavensInteractionWithPlayer : MonoBehaviour
{
    bool isColliding;
    public GameObject UICanvas;
    public GameObject cam;

    public TMP_Text shiny;

    public GameObject itemStore, clothesStore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Entering());
            shiny.text = GameManager.shinyThings.ToString("F0");
            UICanvas.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isColliding = false;
            StartCoroutine(Entering());
            UICanvas.SetActive(false);
        }
    }

    IEnumerator Entering()
    {
        yield return new WaitForSeconds(.25f);
        if (isColliding)
            cam.GetComponent<Animator>().SetBool("focused", true);
        else
            cam.GetComponent<Animator>().SetBool("focused", false);

    }

    public void itemPressed()
    {
        itemStore.SetActive(true);
        clothesStore.SetActive(false);
    }

    public void clothesPressed()
    {
        itemStore.SetActive(false);
        clothesStore.SetActive(true);
    }
}
