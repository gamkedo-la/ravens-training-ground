using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    [Header("Positional")]
    public Transform startPosition;
    public Transform endPosition;
    public Transform parent;
    public AnimationCurve positionAnimationCurve;
    [Header("Text")]
    public TMP_Text damageText;
    public AnimationCurve textColorAnimationCurve;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            PlayTextAnimation();
        }
    }

    public void PlayTextAnimation()
    {
        StartCoroutine(RunAnimation());
    }

    void SetTextColor(float progress)
    {
        Color textColor = Color.white;

        textColor.a = textColorAnimationCurve.Evaluate(progress);

        damageText.color = textColor;
    }
    void SetPosition(float progress)
    {
        parent.position = Vector3.Lerp(startPosition.position, endPosition.position,positionAnimationCurve.Evaluate(progress));
    }
    IEnumerator RunAnimation()
    {
        float progress = 0;

        transform.position = Vector3.zero;


        Color textColor = Color.white;

        while (progress <= 1f)
        {

            SetTextColor(progress);

            SetPosition(progress);


            progress += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
    }
}
