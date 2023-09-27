using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSliderHandlePositioner : MonoBehaviour
{
    [SerializeField] Transform topPosition;
    [SerializeField] Transform bottomPosition;
    [SerializeField] Transform controlPoint1;
    [SerializeField] Transform controlPoint2;
    [SerializeField] Transform handleImageTransform;

    // Start is called before the first frame update
    void Start()
    {
        float startVolume = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : AudioListener.volume;
        SetHandlePosition(startVolume);
    }

    public void SetHandlePosition(float value)
    {
        Vector3 handlePosition = BezierInterpolate(value);
        handleImageTransform.position = handlePosition;
    }

    private Vector3 BezierInterpolate(float value)
    {
        Vector3 bottomToControl1 = Vector3.Lerp(bottomPosition.position, controlPoint1.position, value);
        Vector3 control1ToTop = Vector3.Lerp(controlPoint1.position, topPosition.position, value);
        Vector3 interPosition1 = Vector3.Lerp(bottomToControl1, control1ToTop, value);

        Vector3 control1ToControl2 = Vector3.Lerp(controlPoint1.position, controlPoint2.position, value);
        Vector3 control2ToTop = Vector3.Lerp(controlPoint2.position, topPosition.position, value);
        Vector3 interPosition2 = Vector3.Lerp(control1ToControl2, control2ToTop, value);

        Vector3 handlePosition = Vector3.Lerp(interPosition1, interPosition2, value);
        return handlePosition;
    }

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            Vector3 gizmoPosition = BezierInterpolate(t);
            Gizmos.DrawSphere(gizmoPosition, 2f);
        }

        Gizmos.DrawLine(bottomPosition.position, controlPoint1.position);
        Gizmos.DrawLine(controlPoint2.position, topPosition.position);
    }
}
