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

    private void Awake()
    {
        StartCoroutine(RunAnimation());
    }

    public IEnumerator Create(string text,GameObject incomingGameObject)
    {
        GameObject temp =Instantiate(this).gameObject;
        temp.transform.position = incomingGameObject.transform.position;
        temp.transform.LookAt(Camera.main.transform);
        SetText(text);

        yield return new WaitForEndOfFrame();

    }
    public void PlayTextAnimation()
    {
        StartCoroutine(RunAnimation());
    }
    void SetText(string text)
    {
        damageText.text = text;
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
    public IEnumerator RunAnimation()
    {
        float progress = 0;

        transform.position = Vector3.zero;

        while (progress <= 1f)
        {

            SetTextColor(progress);

            SetPosition(progress);


            progress += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
