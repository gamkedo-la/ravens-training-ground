using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dungeonChest : MonoBehaviour
{
    bool hasEntered;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (hasEntered && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("hasOpened", true);
            print("OpenChest");
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
}
