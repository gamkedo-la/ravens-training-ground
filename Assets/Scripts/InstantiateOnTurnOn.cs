using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateOnTurnOn : MonoBehaviour
{
    public GameObject objToInstantiate;
    bool countDown;
    public float timerToTurnOff;

    void Update()
    {
        if (gameObject.activeSelf)
        {
            countDown = true;
            Instantiate(objToInstantiate, transform.position, transform.rotation);
        }

        if (countDown)
        {
            timerToTurnOff -= Time.deltaTime;
        }

        if (timerToTurnOff <= 0)
            gameObject.SetActive(false);
    }
}
