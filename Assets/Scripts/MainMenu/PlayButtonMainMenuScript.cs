using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonMainMenuScript : MonoBehaviour
{
    [SerializeField] Image fadeToBlackImage;
    [SerializeField] ParticleSystem ravensFlyIn;
    [SerializeField] float timeToNextLevel = 2f;
    [SerializeField] float timeToNextParticle = 0.5f;

    private bool isFading = false;

    private void Update()
    {
        if(!isFading) return;

        Color fadeToBlackImageColor = fadeToBlackImage.color;
        fadeToBlackImageColor.a += 1f/timeToNextLevel * Time.deltaTime;
        fadeToBlackImage.color = fadeToBlackImageColor;
    }

    public void HandleClick()
    {
        StartCoroutine(FadeToFloor1Level());
    }

    private IEnumerator FadeToFloor1Level()
    {
        InstantiateRavens();
        yield return new WaitForSeconds(timeToNextParticle);
        isFading = true;
        yield return new WaitForSeconds(timeToNextLevel);
        SceneManager.LoadScene("IntroCutscene");
    }

    private void InstantiateRavens()
    {
        Transform camera = FindObjectOfType<Camera>().transform;

        foreach(Transform child in camera)
        {
            StartCoroutine(InstantiateParticleAndWait(child));
        }
    }

    private IEnumerator InstantiateParticleAndWait(Transform child)
    {
        ParticleSystem ravensInstance = Instantiate(ravensFlyIn, child) as ParticleSystem;
        ParticleSystem.MainModule ravensInstanceMain = ravensInstance.main;
        ravensInstanceMain.loop = true;
        ravensInstance.transform.localPosition = Vector3.zero;
        ravensInstance.transform.localRotation = Quaternion.identity;
        yield return new WaitForSeconds(timeToNextParticle);
    }
}
