using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool isGamePaused = false;
    [SerializeField] GameObject pauseMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandlePauseEvents();
    }

    private void HandlePauseEvents()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isGamePaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0.0f;
        pauseMenuCanvas.SetActive(true);
    }


    private void UnPauseGame()
    {
        isGamePaused = false;
        Time.timeScale = 1.0f;
        pauseMenuCanvas.SetActive(false);
    }
}
