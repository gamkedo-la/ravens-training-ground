using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOpenGameObject : MonoBehaviour
{
    public GameObject objectToOpen;
    public string tagOfObject;

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.tag == tagOfObject)
            objectToOpen.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == tagOfObject)
            objectToOpen.SetActive(false);
    }
}
