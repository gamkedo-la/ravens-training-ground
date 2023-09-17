using Character.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayerProfileController : MonoBehaviour
{
    public Unit unit;
    public Slider healthUI;
    public Slider manaUI;
    public TMP_Text healthText;
    public TMP_Text manaText;
    public Image icon;
    public Image triangle;

    public void Intitialize(Unit unit,int index)
    {
        this.unit = unit;
        unit.GetComponent<Health>().HealthChangedEvent += UpdateHealth;
        unit.GetComponent<Magic>().MagicPointsChangedEvent += UpdateMagic;

        SetIcons(index);
        SetHealth();
        SetMana();
    }
    public void SetIcons(int index)
    {
        triangle.color = GameManager.colorsInParty[index];
        icon.color = Color.white;
        icon.sprite = Resources.Load<Sprite>("PlayerIcons/" + unit.Name);
    }
    public void SetHealth()
    {
        healthUI.maxValue = unit.GetComponent<Health>().GetMaxHP();
        healthUI.value = unit.GetComponent<Health>().GetCurrentHP();
        healthText.text = unit.GetComponent<Health>().GetCurrentHP().ToString("F0");
    }
    public void SetMana()
    {
        manaUI.maxValue = unit.GetComponent<Magic>().GetMaxMP();
        manaUI.value = unit.GetComponent<Magic>().GetCurrentMP();
        manaText.text = unit.GetComponent<Magic>().GetCurrentMP().ToString("F0");
    }
    public void UpdateHealth(int health)
    {
        healthUI.value = health;
        healthText.text = health.ToString("F0");
    }
    public void UpdateMagic(int magic)
    {
        manaUI.value = magic;
        manaText.text = magic.ToString("F0");
    }

}

