using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToLookAt : MonoBehaviour
{
    bool collided;

    private void Update()
    {
        if (collided)
            if (Input.GetKeyDown(KeyCode.Space))
                print("this character added to party");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            collided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            collided = false;
    }
}
