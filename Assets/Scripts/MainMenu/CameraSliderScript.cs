using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSliderScript : MonoBehaviour
{
    [SerializeField] Slider mySlider;
    [SerializeField] Camera gameCamera;

    [SerializeField] GameObject persistentDataObject;
    private PersistentDataScript persistentDataScript;

    // Start is called before the first frame update
    void Start()
    {
        persistentDataScript = persistentDataObject.GetComponent<PersistentDataScript>();

        if (PlayerPrefs.HasKey("CameraSensitivity"))
        {
            mySlider.value = PlayerPrefs.GetFloat("CameraSensitivity");
        }
        else
        {      
            mySlider.value = persistentDataScript.cameraSensitivity;
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("CameraSensitivity", mySlider.value);
        persistentDataScript.cameraSensitivity = mySlider.value;
    }
}
