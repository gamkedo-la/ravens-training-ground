using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataScript : MonoBehaviour
{
    public float gameVolume;
    public string gameDifficulty;
    public float cameraSensitivity;

    [SerializeField] GameObject difficultySection;
    private DifficultySectionScript difficultySectionScript;

    private bool gameDifficultyHasBeenInitialized = false;
    private bool cameraSensitivityHasBeenInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        difficultySectionScript = difficultySection.GetComponent<DifficultySectionScript>();

        InitializePersistentData();
    }

    private void InitializeGameDifficulty()
    {
        if (PlayerPrefs.HasKey("GameDifficulty"))
        {
            gameDifficulty = PlayerPrefs.GetString("GameDifficulty");
        }
        else
        {
            gameDifficulty = "Medium";
        }

        difficultySectionScript.InitializeGameDifficulty(gameDifficulty);

        gameDifficultyHasBeenInitialized = true;
    }    

    private void InitializeCameraSensitivity()
    {
        if (PlayerPrefs.HasKey("CameraSensitivity"))
        {
            cameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity");
        }
        else
        {
            cameraSensitivity = 0.5f;
        }

        cameraSensitivityHasBeenInitialized = true;
    }    

    private void InitializePersistentData()
    {
        if (!gameDifficultyHasBeenInitialized)
        {
            InitializeGameDifficulty();
        }

        if (!cameraSensitivityHasBeenInitialized)
        {
            InitializeCameraSensitivity();
        }
    }    
}
