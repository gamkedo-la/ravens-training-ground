using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotateBob : MonoBehaviour
{
    public float percentMin = .9f, percentMax = 1.1f;
    public float multiplier = 8;
    public bool rotation, bob, grow;
    public Vector3 rotateDirection;

    Vector3 minVect3, maxVect3, defaultVect3;
    public float startingX, startingY, startingZ;

    public bool getStartRot;

    public float rotationSpeed = 100f;

    private void Start()
    {
        minVect3 = new Vector3(startingX * percentMin, startingY * percentMin, startingZ * percentMin);
        maxVect3 = new Vector3(startingX * percentMax, startingY * percentMax, startingZ * percentMax);

        if (getStartRot)
        {
            startingX = this.transform.rotation.x;
            startingY = this.transform.rotation.y;
            startingZ = this.transform.rotation.z;
        }
    }

    void Update()
    {
        if (bob)
        {
            if (grow)
                this.gameObject.transform.position += defaultVect3 * .1f * multiplier * Time.deltaTime;
            else
                this.gameObject.transform.position += defaultVect3 * .1f * -multiplier * Time.deltaTime;

            if (this.gameObject.transform.position.x >= maxVect3.x)
                grow = false;
            if (this.gameObject.transform.position.x <= minVect3.x)
                grow = true;
        }
        if (rotation)
        {
            transform.Rotate(rotateDirection * rotationSpeed * Time.deltaTime);
        }
    }
}
