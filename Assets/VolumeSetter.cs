using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    private void Start()
    {
        float startVolume = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : AudioListener.volume;
        GetComponent<Slider>().value = startVolume;
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
}
