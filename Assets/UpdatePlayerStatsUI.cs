using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Character.Stats;

public class UpdatePlayerStatsUI : MonoBehaviour
{
    public Health[] playersHealth;
    public Magic[] playersMagic;
    public TMP_Text[] playerCurrentHealth, playerCurrentMP;

    private void Start()
    {
        UpdatePlayerUI();
    }

    public void UpdatePlayerUI()
    {
        for (int i = 0; i < playerCurrentHealth.Length; i++)
        {
            playerCurrentHealth[i].text = playersHealth[i].HitPoints.ToString("F0") + " / " + playersHealth[i].startingHitPoints.ToString("F0");

            playerCurrentMP[i].text = playersMagic[i].MagicPoints.ToString("F0") + " / " + playersMagic[i].startingMagicPoints.ToString("F0");
        }
    }
}
