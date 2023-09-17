using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    private bool isGamePaused = false;
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] PlayableDirector pauseTimeline;
    [SerializeField] GameObject startingMenu;
    [SerializeField] GameObject foreground;
    [SerializeField] GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        ResetPauseCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePauseEvents();
    }

    public void ResumeGame()
    {
        ResetTimeline();
        ResetPauseCanvas();
        UnFreezeGame();
    }

    private void PauseGame()
    {
        FreezeGame();
        pauseMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        pauseTimeline.Play();
    }

    private void HandlePauseEvents()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.ClearGameManagerDataFromInventory();

            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void ResetPauseCanvas()
    {
        foreach (Transform canvasObject in pauseMenuCanvas.transform)
        {
            if(
                canvasObject.gameObject == startingMenu ||
                canvasObject.gameObject == foreground ||
                canvasObject.gameObject == background
            )
            {
                canvasObject.gameObject.SetActive(true);
            }
            else
            {
                canvasObject.gameObject.SetActive(false);
            }
        }
        DeactivateStartingButtons();
        pauseMenuCanvas.SetActive(false);
    }

    private void ResetTimeline()
    {
        pauseTimeline.Stop();
        pauseTimeline.time = 0;
        pauseTimeline.Evaluate();
    }

    private void FreezeGame()
    {
        isGamePaused = true;
        Time.timeScale = 0.0f;
    }

    private void UnFreezeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1.0f;
    }

    public void ActivatePauseButtons()
    {
        ActivateStartingButtons();
    }

    private void ActivateStartingButtons()
    {
        foreach (Button startingMenuButton in startingMenu.GetComponentsInChildren<Button>())
        {
            startingMenuButton.enabled = true;
        }
    }

    private void DeactivateStartingButtons()
    {
        foreach (Transform startingMenuButton in startingMenu.transform)
        {
            startingMenuButton.GetComponent<Button>().enabled = false;
        }
    }

    public void QuitGame()
    {
        UnFreezeGame();
        SceneManager.LoadScene("MainMenu");
    }
}
