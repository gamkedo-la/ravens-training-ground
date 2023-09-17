using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraController : MonoBehaviour
{
    public Camera BattleCamera;

    public void SetBattleCameraPosition(Vector3 position)
    {
        BattleCamera.transform.position = position;
    }
    public void SetBattleCameraRotation(Vector3 eulerAngles)
    {
        BattleCamera.transform.eulerAngles = eulerAngles;
    }
    public void SetBattleCameraTransform(Transform transform)
    {
        SetBattleCameraPosition(transform.position);
        SetBattleCameraRotation(transform.eulerAngles);
    }
}
