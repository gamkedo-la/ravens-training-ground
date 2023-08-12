using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavensInteractionWithPlayer : MonoBehaviour
{
    bool isColliding;
    public GameObject UICanvas;
    public GameObject cam;
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
}
