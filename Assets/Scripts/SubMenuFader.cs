using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuFader : MonoBehaviour
{
    public bool isFadingIn = false;
    public bool isFadingOut = false;
    private float fadeSpeed = 1f;

    private CanvasGroup myCanvasGroupComponent;

    [SerializeField] GameObject mainMenuCanvas;
    private MainMenuScript mainMenuScript;

    // Start is called before the first frame update
    void Start()
    {
        myCanvasGroupComponent = gameObject.transform.GetComponent<CanvasGroup>();
        mainMenuScript = mainMenuCanvas.gameObject.transform.GetComponent<MainMenuScript>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleFadeInWhenAppropriate();
        HandleFadeOutWhenAppropriate();
    }

    private void HandleFadeInWhenAppropriate()
    {
        if (isFadingIn)
        {
            myCanvasGroupComponent.alpha += fadeSpeed * Time.deltaTime;

            if (myCanvasGroupComponent.alpha >= 1)
            {
                myCanvasGroupComponent.alpha = 1;
                isFadingIn = false;
                gameObject.SetActive(true);
                mainMenuCanvas.gameObject.SetActive(false);
            }
        }
    }

    private void HandleFadeOutWhenAppropriate()
    {
        if (isFadingOut)
        {
            myCanvasGroupComponent.alpha -= fadeSpeed * Time.deltaTime;

            if (myCanvasGroupComponent.alpha <= 0)
            {
                myCanvasGroupComponent.alpha = 0;
                isFadingOut = false;
                gameObject.SetActive(false);
                mainMenuCanvas.gameObject.SetActive(true);
                mainMenuScript.isFadingIn = true;
            }
        }
    }
}
