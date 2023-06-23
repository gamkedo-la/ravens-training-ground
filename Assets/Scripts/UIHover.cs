using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float startingX, startingY, startingZ;
    public float percentMin = .9f, percentMax = 1.1f;
    public float multiplier = 8;
    Vector3 minVect3, maxVect3, defaultVect3;
    bool grow, hover;

    void Start()
    {
        grow = true;
        minVect3 = new Vector3(startingX * percentMin, startingY * percentMin, startingZ * percentMin);
        maxVect3 = new Vector3(startingX * percentMax, startingY * percentMax, startingZ * percentMax);
        defaultVect3 = new Vector3(startingX, startingY, startingZ);
    }

    void Update()
    {
        if (hover)
        {
            if (grow)
                this.gameObject.transform.localScale += defaultVect3 * .1f * multiplier * Time.deltaTime;
            else
                this.gameObject.transform.localScale += defaultVect3 * .1f * -multiplier* Time.deltaTime;

            if (this.gameObject.transform.localScale.x >= maxVect3.x)
                grow = false;
            if(this.gameObject.transform.localScale.x <= minVect3.x)
                grow = true;
        }

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;

        Debug.Log("The cursor entered the selectable UI element.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
        this.gameObject.transform.localScale = defaultVect3;
        Debug.Log("Cursor has exited.");
    }
}
