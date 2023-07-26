using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
    void Update()
    {
        if (!Camera.main || Camera.main == null) return;
        Vector3 oppositeCamera = transform.position - Camera.main.transform.position;
        Quaternion cameraRot = Quaternion.LookRotation(oppositeCamera);
        Vector3 euler = cameraRot.eulerAngles;
        euler.y = 0f;
        cameraRot.eulerAngles = euler;
        transform.rotation = cameraRot;
    }
}
