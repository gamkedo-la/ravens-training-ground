using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToLoadBoss : MonoBehaviour
{
    public Animator anim;

    public float timer;
    bool startTimer;
    public GameObject oldCam, newCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetTrigger("Attack");
            startTimer = true;
            newCam.SetActive(true);
            oldCam.SetActive(false);
        }
    }

    private void Update()
    {
        if (startTimer)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SceneManager.LoadScene("BattleSceneFloor3");
            }
        }
    }
}
