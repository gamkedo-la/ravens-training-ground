using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotateBob : MonoBehaviour
{
    public bool rotation, bob;
    public Vector3 rotateDirection;
    public float minHeight, maxHeight;
    float speed;

    void Update()
    {
        if (bob)
        {
            //this.transform.position.y += speed * Time.deltaTime;
        }
        if (rotation)
        {
            transform.Rotate(rotateDirection);
        }
    }
}
