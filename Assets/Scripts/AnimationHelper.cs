using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public GameObject parent;
    public string functionName, functionNameTwo;

    public void AnimationToTrigger()
    {
        parent.GetComponent<Unit>().Invoke(functionName, 0f);
    }

    public void SecondAnimationToTrigger()
    {
        parent.GetComponent<Unit>().Invoke(functionNameTwo, 0f);
    }
}
