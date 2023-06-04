using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUnit : MonoBehaviour
{
    public BattleSystem.CharacterIdentifier myEnumValue;
    public BattleSystem battleSystem;

    public bool isDani, isErebus, isPhebe, isTristan;

    public string unitName;
    public bool isCherufe, isFresno, isMiniwashitu, isPukwudgie, isSoucouyant, isSquonk;

    public float unitMaxHP;
    public float unitCurrentHP;

    public float attackDamage;

    public bool enemy;

    public int agility;

    public TextMeshProUGUI nameUI;
    public GameObject[] TransAffinity, CharmsAffinity, PotionsAffinity, PhysicalAffinity, DarkArtsAffinity, AncientAffinity;

    public bool Trans, Charms, Potions, Phys, DA, Ancient, Ultimate;

    //Determining if Players or Enemies are Strong, Weak, Absorb, or Reflect of - 
    //T = Transfiguration, C = Charms, Po = Potions, Ph = Physical, D = Dark Arts, A = Ancient
    public bool StrT, StrC, StrPo, StrPh, StrD, StrA;
    public bool WeakT, WeakC, WeakPo, WeakPh, WeakD, WeakA;
    public bool RefT, RefC, RefPo, RefPh, RefD, RefA;
    public bool AbsT, AbsC, AbsPo, AbsPh, AbsD, AbsA;

    bool GMTriggerT, GMTriggerC, GMTriggerPo, GMTriggerPh, GMTriggerD, GMTriggerA;

    public bool criticalLanded;
    float criticalMultiplier;
    public TextMeshProUGUI criticalText;

    //SustainedItems
    public bool isParalyzed;
    public int paralyzedCounter;

    public int DefenseCounter;
    public float defensePercent = 1;

    public int AttackCounter;
    public float attackPercent = 1;

    public GameObject informationUI;

    public Slider healthSlider;

    public Animator anim;

    bool hasSpawnedParticle;
    public GameObject deathParticle;

    float totalDamage;

    public TextMeshProUGUI affinityText;
    public Image sliderForeground;

    public bool isPoisoned;
    public GameObject poisonedParticle;
    public int poisonedTurnCount;
    public float poisonedDamage;

    public bool isDizzy;
    public GameObject dizzyParticle;
    public int dizzyTurnCount;

    public TextMeshProUGUI dmgTxt;

    public List<string> attacksKnown = new List<string>();
    public string[] attacksCurrentlyKnown;

    private void Start()
    {
        for (int i = 0; i < attacksCurrentlyKnown.Length; i++)
        {
            attacksKnown.Add(attacksCurrentlyKnown[i]);
        }

        battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        if (isSoucouyant)
            anim = GameObject.Find("Demon").GetComponent<Animator>();
        else
            anim = GetComponentInChildren<Animator>();
        agility = 1;
        if (enemy)
        {
            healthSlider.value = unitCurrentHP / unitMaxHP;

            nameUI.text = unitName;

            #region Enemy Affinities
            /*
                        for (int i = 0; i < TransAffinity.Length; i++)
                        {
                            TransAffinity[i].SetActive(false);
                        }
                        for (int i = 0; i < CharmsAffinity.Length; i++)
                        {
                            CharmsAffinity[i].SetActive(false);
                        }
                        for (int i = 0; i < PotionsAffinity.Length; i++)
                        {
                            PotionsAffinity[i].SetActive(false);
                        }
                        for (int i = 0; i < PhysicalAffinity.Length; i++)
                        {
                            PhysicalAffinity[i].SetActive(false);
                        }
                        for (int i = 0; i < DarkArtsAffinity.Length; i++)
                        {
                            DarkArtsAffinity[i].SetActive(false);
                        }
                        for (int i = 0; i < AncientAffinity.Length; i++)
                        {
                            AncientAffinity[i].SetActive(false);
                        }
            */
            #region Strong
            if (StrT)
            {
                TransAffinity[1].SetActive(true);
            }
            if (StrC)
            {
                CharmsAffinity[1].SetActive(true);
            }
            if (StrT)
            {
                TransAffinity[1].SetActive(true);
            }
            if (StrPo)
            {
                PotionsAffinity[1].SetActive(true);
            }
            if (StrPh)
            {
                PhysicalAffinity[1].SetActive(true);
            }
            if (StrD)
            {
                DarkArtsAffinity[1].SetActive(true);
            }
            if (StrA)
            {
                AncientAffinity[1].SetActive(true);
            }
            #endregion

            #region Weak
            if (WeakT)
            {
                TransAffinity[2].SetActive(true);
            }
            if (WeakC)
            {
                CharmsAffinity[2].SetActive(true);
            }
            if (WeakT)
            {
                TransAffinity[2].SetActive(true);
            }
            if (WeakPo)
            {
                PotionsAffinity[2].SetActive(true);
            }
            if (WeakPh)
            {
                PhysicalAffinity[2].SetActive(true);
            }
            if (WeakD)
            {
                DarkArtsAffinity[2].SetActive(true);
            }
            if (WeakA)
            {
                AncientAffinity[2].SetActive(true);
            }
            #endregion

            #region Reflect
            if (RefT)
            {
                TransAffinity[3].SetActive(true);
            }
            if (RefC)
            {
                CharmsAffinity[3].SetActive(true);
            }
            if (RefT)
            {
                TransAffinity[3].SetActive(true);
            }
            if (RefPo)
            {
                PotionsAffinity[3].SetActive(true);
            }
            if (RefPh)
            {
                PhysicalAffinity[3].SetActive(true);
            }
            if (RefD)
            {
                DarkArtsAffinity[3].SetActive(true);
            }
            if (RefA)
            {
                AncientAffinity[3].SetActive(true);
            }
            #endregion

            #region Absorb
            if (AbsT)
            {
                TransAffinity[4].SetActive(true);
            }
            if (AbsC)
            {
                CharmsAffinity[4].SetActive(true);
            }
            if (AbsT)
            {
                TransAffinity[4].SetActive(true);
            }
            if (AbsPo)
            {
                PotionsAffinity[4].SetActive(true);
            }
            if (AbsPh)
            {
                PhysicalAffinity[4].SetActive(true);
            }
            if (AbsD)
            {
                DarkArtsAffinity[4].SetActive(true);
            }
            if (AbsA)
            {
                AncientAffinity[4].SetActive(true);
            }
            #endregion

            #region Neutral
            if (!StrT && !WeakT && !RefT && !AbsT)
            {
                TransAffinity[0].SetActive(true);
            }
            if (!StrC && !WeakC && !RefC && !AbsC)
            {
                CharmsAffinity[0].SetActive(true);
            }
            if (!StrPo && !WeakPo && !RefPo && !AbsPo)
            {
                PotionsAffinity[0].SetActive(true);
            }
            if (!StrPh && !WeakPh && !RefPh && !AbsPh)
            {
                PhysicalAffinity[0].SetActive(true);
            }
            if (!StrD && !WeakD && !RefD && !AbsD)
            {
                DarkArtsAffinity[0].SetActive(true);
            }
            if (!StrA && !WeakA && !RefA && !AbsA)
            {
                AncientAffinity[0].SetActive(true);
            }
            #endregion

            #endregion
        }
    }

    public bool TakeDamage(float dmg)
    {
        // Testing purposes, this is a DA attack
        GMTriggerD = true;
        totalDamage = dmg;
        if (criticalLanded)
        {
            criticalMultiplier = 1.5f;
            criticalText.color = Color.magenta;
            criticalText.text = "Critical!";
            if (!enemy)
                battleSystem.friendshipBonus++;
        }
        else
            criticalMultiplier = 1.0f;

        //CURRENTLY NOT WORKING FOR PLAYERS, NO TEXT ABOVE HEADS

        if (Trans)
        {
            if (StrT)
            {
                totalDamage *= (criticalMultiplier * .5f * (1 - defensePercent));
                affinityText.color = Color.black;
                affinityText.text = "Resist!";
            }
            else if (WeakT)
            {
                totalDamage *= (criticalMultiplier * 2f * (1 - defensePercent));
                affinityText.color = Color.yellow;
                affinityText.text = "Weak!";
                battleSystem.friendshipBonus++;
            }
            else if (AbsT)
            {
                totalDamage *= -1f;
                affinityText.color = Color.cyan;
                affinityText.text = "Absorb!";
            }
            else if (RefT)
            {
                affinityText.color = Color.white;
                affinityText.text = "Reflect!";
            }
            else
            {
                totalDamage *= (criticalMultiplier * 1f * (1 - defensePercent));
                print("Other!");
            }
        }

        else if (Charms)
        {
            if (StrC)
            {
                totalDamage *= (criticalMultiplier * .5f * (1 - defensePercent));
                affinityText.color = Color.black;
                affinityText.text = "Resist!";
            }
            else if (WeakC)
            {
                totalDamage *= (criticalMultiplier * 2f * (1 - defensePercent));
                affinityText.color = Color.yellow;
                affinityText.text = "Weak!";
                battleSystem.friendshipBonus++;
            }
            else if (AbsC)
            {
                totalDamage *= -1f;
                affinityText.color = Color.cyan;
                affinityText.text = "Absorb!";
            }
            else if (RefC)
            {
                affinityText.color = Color.white;
                affinityText.text = "Reflect!";
            }
            else
            {
                totalDamage *= (criticalMultiplier * 1f * (1 - defensePercent));
                print("Other!");
            }
        }

        else if (Potions)
        {
            if (StrPo)
            {
                totalDamage *= (criticalMultiplier * .5f * (1 - defensePercent));
                affinityText.color = Color.black;

                if (isPoisoned)
                {
                    affinityText.text = "Immune!";
                    affinityText.color = Color.magenta;
                }
                else
                {
                    affinityText.text = "Resist!";
                }
            }
            else if (WeakPo)
            {
                totalDamage *= (criticalMultiplier * 2f * (1 - defensePercent));
                affinityText.color = Color.yellow;
                affinityText.text = "Weak!";
                battleSystem.friendshipBonus++;
            }
            else if (AbsPo)
            {
                totalDamage *= -1f;
                affinityText.color = Color.cyan;
                affinityText.text = "Absorb!";
            }
            else if (RefPo)
            {
                affinityText.color = Color.white;
                affinityText.text = "Reflect!";
            }
            else
            {
                totalDamage *= (criticalMultiplier * 1f * (1 - defensePercent));
                print("Other!");
            }
        }

        else if (Phys)
        {
            if (StrPh)
            {
                totalDamage *= (criticalMultiplier * .5f * (1 - defensePercent));
                affinityText.color = Color.black;
                affinityText.text = "Resist!";
            }
            else if (WeakPh)
            {
                totalDamage *= (criticalMultiplier * 2f * (1 - defensePercent));
                affinityText.color = Color.yellow;
                affinityText.text = "Weak!";
                battleSystem.friendshipBonus++;
            }
            else if (AbsPh)
            {
                totalDamage *= -1f;
                affinityText.color = Color.cyan;
                affinityText.text = "Absorb!";
            }
            else if (RefPh)
            {
                affinityText.color = Color.white;
                affinityText.text = "Reflect!";
            }
            else
            {
                totalDamage *= (criticalMultiplier * 1f * (1 - defensePercent));
                print("Other!");
            }
        }

        else if (DA)
        {
            if (StrD)
            {
                totalDamage *= (criticalMultiplier * .5f * (1 - defensePercent));
                affinityText.color = Color.black;
                affinityText.text = "Resist!";
            }
            else if (WeakD)
            {
                totalDamage *= (criticalMultiplier * 2f * (1 - defensePercent));
                affinityText.color = Color.yellow;
                affinityText.text = "Weak!";
                battleSystem.friendshipBonus++;
            }
            else if (AbsD)
            {
                totalDamage *= -1f;
                affinityText.color = Color.cyan;
                affinityText.text = "Absorb!";
            }
            else if (RefD)
            {
                affinityText.color = Color.white;
                affinityText.text = "Reflect!";
            }
            else
            {
                totalDamage *= (criticalMultiplier * 1f * (1 - defensePercent));
                print("Other!");
            }
        }

        else if (Ancient)
        {
            if (StrA)
            {
                totalDamage *= (criticalMultiplier * .5f * (1 - defensePercent));
                affinityText.color = Color.black;
                affinityText.text = "Resist!";
            }
            else if (WeakA)
            {
                totalDamage *= (criticalMultiplier * 2f * (1 - defensePercent));
                affinityText.color = Color.yellow;
                affinityText.text = "Weak!";
                battleSystem.friendshipBonus++;
            }
            else if (AbsA)
            {
                totalDamage *= -1f;
                affinityText.color = Color.cyan;
                affinityText.text = "Absorb!";
            }
            else if (RefA)
            {
                affinityText.color = Color.white;
                affinityText.text = "Reflect!";
            }
            else
            {
                totalDamage *= (criticalMultiplier * 1f * (1 - defensePercent));
                print("Other!");
            }
        }
    /*    CheckSliderStatus();
        //Reflect happens in Battle System
        dmgTxt.text = totalDamage.ToString("F0");
        StartCoroutine(TurnOffText());
        //Add the text once this works

        UpdateGameManagerQuestion();
    */
        if (unitCurrentHP >= unitMaxHP)
        {
            unitCurrentHP = unitMaxHP;
            return false;
        }

        else if (unitCurrentHP <= 0)
        {
            if (!hasSpawnedParticle)
            {
                Instantiate(deathParticle, transform.position + new Vector3(0, 1f, 0), transform.rotation);
            }
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);

            hasSpawnedParticle = true;
            return true;
        }

        else
        {
            print("LookyLooky");
            dmgTxt.transform.localScale = new Vector3(1f, 1f, 1f);

            anim.SetTrigger("TakeDamage");
            return false;
        }
    }

    IEnumerator TurnOffText()
    {
        dmgTxt.color = Color.red;

        yield return new WaitForSeconds(1);
        affinityText.text = "";
        affinityText.color = Color.white;
        criticalText.text = "";

        dmgTxt.text = "";
        dmgTxt.color = Color.red;
        dmgTxt.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
  /*  void CheckSliderStatus()
    {
        if (isDani)
        {
            healthSlider = GameObject.Find("DaniHealthSlider").GetComponent<Slider>();
            sliderForeground = GameObject.Find("DaniForeground").GetComponent<Image>();
            GameManager.DaniCurrentHP -= totalDamage;
        }
        else if (isErebus)
        {
            healthSlider = GameObject.Find("ErebusHealthSlider").GetComponent<Slider>();
            sliderForeground = GameObject.Find("ErebusForeground").GetComponent<Image>();
            GameManager.ErebusCurrentHP -= totalDamage;
        }
        else if (isPhebe)
        {
            healthSlider = GameObject.Find("PhebeHealthSlider").GetComponent<Slider>();
            sliderForeground = GameObject.Find("PhebeForeground").GetComponent<Image>();
            GameManager.PhoebeCurrentHP -= totalDamage;
        }
        else if (isTristan)
        {
            healthSlider = GameObject.Find("TristanHealthSlider").GetComponent<Slider>();
            sliderForeground = GameObject.Find("TristanForeground").GetComponent<Image>();
            GameManager.TristanCurrentHP -= totalDamage;
        }
        else
        {
            unitCurrentHP -= totalDamage;

            healthSlider.value = unitCurrentHP / unitMaxHP;
        }

        //if health <50% and >20% make yellow
        //if health <20% make it red
        //else green
        if (enemy)
        {
            if (unitCurrentHP > (unitMaxHP * .5f))
            {
                sliderForeground.color = Color.green;
            }
            else if (unitCurrentHP <= (unitMaxHP * .5f) && unitCurrentHP >= (unitMaxHP * .2f))
            {
                sliderForeground.color = Color.yellow;
            }
            else if (unitCurrentHP < (unitMaxHP * .2f))
            {
                sliderForeground.color = Color.red;
            }
        }
    }

    public void CheckIfAffinityKnown()
    {
        if (isCherufe)
        {
            if (GameManager.CherufeTrans)
                TransAffinity[5].SetActive(false);
            if (GameManager.CherufeCharms)
                CharmsAffinity[5].SetActive(false);
            if (GameManager.CherufePotions)
                PotionsAffinity[5].SetActive(false);
            if (GameManager.CherufePhys)
                PhysicalAffinity[5].SetActive(false);
            if (GameManager.CherufeDA)
                DarkArtsAffinity[5].SetActive(false);
            if (GameManager.CherufeAncient)
                AncientAffinity[5].SetActive(false);
        }

        else if (isFresno)
        {
            if (GameManager.FresnoTrans)
                TransAffinity[5].SetActive(false);
            if (GameManager.FresnoCharms)
                CharmsAffinity[5].SetActive(false);
            if (GameManager.FresnoPotions)
                PotionsAffinity[5].SetActive(false);
            if (GameManager.FresnoPhys)
                PhysicalAffinity[5].SetActive(false);
            if (GameManager.FresnoDA)
                DarkArtsAffinity[5].SetActive(false);
            if (GameManager.FresnoAncient)
                AncientAffinity[5].SetActive(false);
        }

        else if (isMiniwashitu)
        {
            if (GameManager.MiniwashituTrans)
                TransAffinity[5].SetActive(false);
            if (GameManager.MiniwashituCharms)
                CharmsAffinity[5].SetActive(false);
            if (GameManager.MiniwashituPotions)
                PotionsAffinity[5].SetActive(false);
            if (GameManager.MiniwashituPhys)
                PhysicalAffinity[5].SetActive(false);
            if (GameManager.MiniwashituDA)
                DarkArtsAffinity[5].SetActive(false);
            if (GameManager.MiniwashituAncient)
                AncientAffinity[5].SetActive(false);
        }

        else if (isPukwudgie)
        {
            if (GameManager.PukwudgieTrans)
                TransAffinity[5].SetActive(false);
            if (GameManager.PukwudgieCharms)
                CharmsAffinity[5].SetActive(false);
            if (GameManager.PukwudgiePotions)
                PotionsAffinity[5].SetActive(false);
            if (GameManager.PukwudgiePhys)
                PhysicalAffinity[5].SetActive(false);
            if (GameManager.PukwudgieDA)
                DarkArtsAffinity[5].SetActive(false);
            if (GameManager.PukwudgieAncient)
                AncientAffinity[5].SetActive(false);
        }

        else if (isSoucouyant)
        {
            if (GameManager.SoucouyantTrans)
                TransAffinity[5].SetActive(false);
            if (GameManager.SoucouyantCharms)
                CharmsAffinity[5].SetActive(false);
            if (GameManager.SoucouyantPotions)
                PotionsAffinity[5].SetActive(false);
            if (GameManager.SoucouyantPhys)
                PhysicalAffinity[5].SetActive(false);
            if (GameManager.SoucouyantDA)
                DarkArtsAffinity[5].SetActive(false);
            if (GameManager.SoucouyantAncient)
                AncientAffinity[5].SetActive(false);
        }

        else if (isSquonk)
        {
            if (GameManager.SquonkTrans)
                TransAffinity[5].SetActive(false);
            if (GameManager.SquonkCharms)
                CharmsAffinity[5].SetActive(false);
            if (GameManager.SquonkPotions)
                PotionsAffinity[5].SetActive(false);
            if (GameManager.SquonkPhys)
                PhysicalAffinity[5].SetActive(false);
            if (GameManager.SquonkDA)
                DarkArtsAffinity[5].SetActive(false);
            if (GameManager.SquonkAncient)
                AncientAffinity[5].SetActive(false);
        }
    }

    void UpdateGameManagerQuestion()
    {
        if (GMTriggerT)
        {
            if (isCherufe)
                GameManager.CherufeTrans = true;
            else if (isFresno)
                GameManager.FresnoTrans = true;
            else if (isMiniwashitu)
                GameManager.MiniwashituTrans = true;
            else if (isPukwudgie)
                GameManager.PukwudgieTrans = true;
            else if (isSoucouyant)
                GameManager.SoucouyantTrans = true;
            else if (isSquonk)
                GameManager.SquonkTrans = true;
        }

        else if (GMTriggerC)
        {
            if (isCherufe)
                GameManager.CherufeCharms = true;
            else if (isFresno)
                GameManager.FresnoCharms = true;
            else if (isMiniwashitu)
                GameManager.MiniwashituCharms = true;
            else if (isPukwudgie)
                GameManager.PukwudgieCharms = true;
            else if (isSoucouyant)
                GameManager.SoucouyantCharms = true;
            else if (isSquonk)
                GameManager.SquonkCharms = true;
        }

        else if (GMTriggerPo)
        {
            if (isCherufe)
                GameManager.CherufePotions = true;
            else if (isFresno)
                GameManager.FresnoPotions = true;
            else if (isMiniwashitu)
                GameManager.MiniwashituPotions = true;
            else if (isPukwudgie)
                GameManager.PukwudgiePotions = true;
            else if (isSoucouyant)
                GameManager.SoucouyantPotions = true;
            else if (isSquonk)
                GameManager.SquonkPotions = true;
        }

        else if (GMTriggerPh)
        {
            if (isCherufe)
                GameManager.CherufePhys = true;
            else if (isFresno)
                GameManager.FresnoPhys = true;
            else if (isMiniwashitu)
                GameManager.MiniwashituPhys = true;
            else if (isPukwudgie)
                GameManager.PukwudgiePhys = true;
            else if (isSoucouyant)
                GameManager.SoucouyantPhys = true;
            else if (isSquonk)
                GameManager.SquonkPhys = true;
        }

        else if (GMTriggerPh)
        {
            if (isCherufe)
                GameManager.CherufeDA = true;
            else if (isFresno)
                GameManager.FresnoDA = true;
            else if (isMiniwashitu)
                GameManager.MiniwashituDA = true;
            else if (isPukwudgie)
                GameManager.PukwudgieDA = true;
            else if (isSoucouyant)
                GameManager.SoucouyantDA = true;
            else if (isSquonk)
                GameManager.SquonkDA = true;
        }

        else if (GMTriggerA)
        {
            if (isCherufe)
                GameManager.CherufeAncient = true;
            else if (isFresno)
                GameManager.FresnoAncient = true;
            else if (isMiniwashitu)
                GameManager.MiniwashituAncient = true;
            else if (isPukwudgie)
                GameManager.PukwudgieAncient = true;
            else if (isSoucouyant)
                GameManager.SoucouyantAncient = true;
            else if (isSquonk)
                GameManager.SquonkAncient = true;
        }
        GMTriggerT = false;
        GMTriggerPo = false;
        GMTriggerPh = false;
        GMTriggerD = false;
        GMTriggerC = false;
        GMTriggerA = false;

        criticalLanded = false;
    }
}
  */