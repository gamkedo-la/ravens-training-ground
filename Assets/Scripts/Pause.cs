using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Pause : MonoBehaviour
{
    private bool isGamePaused = false;
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] PlayableDirector pauseTimeline;

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
            GameManager.ClearGameManagerDataFromInventory();

            if (isGamePaused)
            {
                UnPauseGame();
                ResetTimeline();
            }
            else
            {
                PauseGame();
                pauseTimeline.Play();
            }
        }
    }

    private void ResetTimeline()
    {
        pauseTimeline.Stop();
        pauseTimeline.time = 0;
        pauseTimeline.Evaluate();
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
