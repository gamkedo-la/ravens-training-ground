using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToLookAt : MonoBehaviour
{
    bool collided;
    GameObject player;
    public bool nameBadge;
    public GameObject cam, nameBadgeObj;
    public GameObject UICanvas;
    public string npcName, npcMessage;
    public Color color;
    public string yes, no;
    public UpdateUIForParty updateUI;

    private void Update()
    {
        if (collided)
        {
            transform.LookAt(player.transform);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UICanvas.SetActive(true);
                updateUI.nameToDisplay = npcName;
                updateUI.descToDisplay = npcMessage;

                updateUI.color = color;

                if (GameManager.inCurrentParty.Count < 4)
                {
                    if (!GameManager.inCurrentParty.Contains(npcName))
                        updateUI.yesToDisplay = "Yes";
                    else
                        updateUI.yesToDisplay = "In Party";
                }
                else
                    updateUI.yesToDisplay = "Party Full";      

                if (GameManager.inCurrentParty.Contains(npcName))
                    updateUI.noToDisplay = "Remove";
                else
                    updateUI.noToDisplay = "No";
            }
        }
        if (nameBadge)
            transform.LookAt(cam.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            nameBadgeObj.SetActive(true);
            player = other.gameObject;
            collided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            nameBadgeObj.SetActive(false);
            collided = false;
            UICanvas.SetActive(false);
        }
    }
}
