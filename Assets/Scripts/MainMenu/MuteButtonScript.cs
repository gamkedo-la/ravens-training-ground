using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MuteButtonScript : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    bool isMuted = false;


    public void HandleMute()
    {
        switch (isMuted)
        {
            case true:
                DisableMute();
                break;
            case false:
                EnableMute();
                break;
            default:
                Debug.LogError("Invalid case for mute state!");
                break;
        }
    }

    void EnableMute()
    {
        Debug.Log("Muting aduio " + mixer.name);
        mixer.SetFloat("MasterVolume", -80);
        isMuted = true;
    }

    void DisableMute()
    {
        Debug.Log("Unmuting mixer " + mixer.name + " with value of " + PlayerPrefs.GetFloat("Volume"));
        mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("Volume"));
        isMuted = false;
    }
}
