using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public GameObject thingToDestroyIfNotThis;
    public float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (thingToDestroyIfNotThis != null)
                Destroy(thingToDestroyIfNotThis);
            else
                Destroy(this.gameObject);
        }
    }
}
