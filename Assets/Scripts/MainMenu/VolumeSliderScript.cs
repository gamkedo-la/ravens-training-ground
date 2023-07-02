using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderScript : MonoBehaviour
{
    [SerializeField] Slider mySlider;
    [SerializeField] AudioSource mainCameraAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            mySlider.value = PlayerPrefs.GetFloat("Volume");
        }    
        else
        {
            mySlider.value = mySlider.maxValue;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        mainCameraAudioSource.volume = mySlider.value;
    }
}
