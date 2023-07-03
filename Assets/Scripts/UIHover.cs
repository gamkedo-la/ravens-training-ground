using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float startingX, startingY, startingZ;
    public float percentMin = .9f, percentMax = 1.1f;
    public float multiplier = 8;
    Vector3 minVect3, maxVect3, defaultVect3;
    bool grow, hover;
    public bool isFlee;
    public string levelToLoad;

    void Start()
    {
        grow = true;
        minVect3 = new Vector3(startingX * percentMin, startingY * percentMin, startingZ * percentMin);
        maxVect3 = new Vector3(startingX * percentMax, startingY * percentMax, startingZ * percentMax);
        defaultVect3 = new Vector3(startingX, startingY, startingZ);

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (isFlee && sceneName == "Floor3")
        {
            GetComponent<Button>().interactable = false;
            GetComponent<UIHover>().enabled = false;
        }
           
            
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
                hover = false;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
        this.gameObject.transform.localScale = defaultVect3;
    }

    public void IsContinue()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
