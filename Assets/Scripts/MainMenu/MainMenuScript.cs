using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public bool isFadingIn = false;
    public bool isFadingOut = false;
    private float fadeSpeed = 1f;

    private CanvasGroup myCanvasGroupComponent;

    // Start is called before the first frame update
    void Start()
    {
        myCanvasGroupComponent = gameObject.transform.GetComponent<CanvasGroup>();
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
            }
        }
    }
}
