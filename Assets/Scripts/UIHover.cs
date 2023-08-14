using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float startingX, startingY, startingZ;
    public float percentMin = .9f, percentMax = 1.1f;
    public float multiplier = 8;
    Vector3 minVect3, maxVect3, defaultVect3;
    bool grow, hover;
    public bool isFlee;
    public string levelToLoad;

    // changes color of background image on hover
    public bool tintImageOnHover  = false;
    public Color outlineImageColor = Color.black;
    public Color backgroundImageColor = Color.white;
    public Color hoverTextColor = Color.black;
    private Color outlineImageColorOriginal = Color.white;
    private Color backgroundImageColorOriginal = Color.white;
    private Color textColorOriginal = Color.black;

    public GameObject toTurnOn;

    void Start()
    {
        grow = true;
        minVect3 = new Vector3(startingX * percentMin, startingY * percentMin, startingZ * percentMin);
        maxVect3 = new Vector3(startingX * percentMax, startingY * percentMax, startingZ * percentMax);
        defaultVect3 = new Vector3(startingX, startingY, startingZ);

        if (tintImageOnHover) {
            outlineImageColorOriginal = GetComponent<Image>().color;
            backgroundImageColorOriginal = this.gameObject.transform.GetChild(0).GetComponent<Image>().color;
            textColorOriginal = GetComponentInChildren<TextMeshProUGUI>().color;
        }

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

        if (tintImageOnHover) {
            GetComponent<Image>().color = outlineImageColor;
            this.gameObject.transform.GetChild(0).GetComponent<Image>().color = backgroundImageColor;
            GetComponentInChildren<TextMeshProUGUI>().color = hoverTextColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
        if (tintImageOnHover) {
            GetComponent<Image>().color = outlineImageColorOriginal;
            this.gameObject.transform.GetChild(0).GetComponent<Image>().color = backgroundImageColorOriginal;
            GetComponentInChildren<TextMeshProUGUI>().color = textColorOriginal;
        }
    }

    public void IsContinue()
    {
        toTurnOn.SetActive(true);
        Invoke("Advance", 1f);
    }

    void Advance()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
