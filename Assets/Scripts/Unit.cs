using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    //HP = Health, MP = Magic, Magic = damage through magic, Physical = damage through physical, agility = ability to dodge, finesse = critical chance
    public bool isOnAuto = true;
    public bool isAPlayer;

    public string Name;
    public float CurrentHP = 25, MaxHP = 20, CurrentMP = 30, MaxMP = 25, Magic = 10, Physical = 9, Agility = 15, Finesse = 20;
    public int CurrentLevel = 1;
    public int tempAgility = 0;

    public float MagicEquipment, PhysicalEquipment, AgilityEquipment, FinesseEquipment;

    public List<Affinity> resistances;
    public List<Affinity> weaknesses;

    float lightAttack = 1f, mediumAttack = 1.4f;

    public bool isASoloAttack, isAGroupAttack, isAHeal;
    bool charms, physical, darkArts, transfiguration, ancient;
    bool attackBoosted, defenseBoosted, charged;

    public int attackBonusTurnCount, defenseBonusTurnCount;

    public float attackMultiplier = 1, defenseMultiplier = 1;

    public float damage, healthToRecover;

    public float experienceEarned;

    public bool hasBeenKnockedDown;

    public Animator anim;
    public GameObject powerUpParticle, deathParticle, targetParticle, canvas;

    //Cost
    int deductCost;
    public int tier0 = 0, tier1 = 4, tier2 = 8, tier3 = 12, tier4 = 15, tier5 = 18, tier6 = 21;

    Battle battle;

    public List<AttackBase> attacks;

    public TMP_Text characterName, affinityText;
    public Slider healthBar;

    public int chanceOfMetallic = 75;
    public bool isMetallic;
    public Material metallic;

    public bool canFlee;
    public int randomAttack;
    public GameObject victoryPlacement;
    public GameObject objectToChangeMaterialOf;
    public Material[] playerMaterial;

    public enum UnitState {
        CASTINGSUPPORTSPELL,
        CASTINGATTACKSPELL,
        IDLE,
        Alive,
        Unconscious,
        Fled,
        Dead
    }

    public UnitState currentState;

    private void Start()
    {
        currentState = UnitState.IDLE;
        battle = GameObject.Find("Battle").GetComponent<Battle>();

        if (!isAPlayer && characterName.text != null)
        {
            characterName.text = Name;

            int percentOfMetallic = Random.Range(0, 100);
            if (percentOfMetallic <= chanceOfMetallic && Name != "Cherufe")
                isMetallic = true;

            if (isMetallic)
            {
               Renderer[] children = GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in children)
                {
                    var mats = new Material[rend.materials.Length];
                    for (var j = 0; j < rend.materials.Length; j++)
                    {
                        mats[j] = metallic;
                    }
                    rend.materials = mats;
                }

                CurrentHP = 5;
                MaxHP = 5;
                Agility = 80;
                Finesse = 80;
                experienceEarned *= 5; 
                canFlee = true;
            }
        }
        
    }

    public void TakingUnitTurn(AttackBase selectedAttack = null)
    {
        //break out if unit is unconcious dead or fled
        if (currentState == UnitState.Unconscious)
        { return; }
        if (currentState == UnitState.Fled)
        { return; }
        if (currentState == UnitState.Dead)
        { return; }
        /*        if (currentState != UnitState.Unconscious || currentState != UnitState.Fled)
                {*/
        battle.MoveCamera();
        if (hasBeenKnockedDown)
        {
            anim.SetBool("knockedDown", false);
            hasBeenKnockedDown = false;
        }

        if (attackBonusTurnCount > 0)
            attackBonusTurnCount--;
        else
            attackMultiplier = 1;

        if (defenseBonusTurnCount > 0)
            defenseBonusTurnCount--;
        else
            defenseMultiplier = 1;

        if (DidNotFlee() == false)
            return;

        AttackBase attackBaseTemp = DetermineAttackFromList(selectedAttack);

       
        if (isOnAuto)
        {
            Unit target = SelectAutoRandomTarget();

            attackBaseTemp.AttemptAttack(this, target);
        }

        // DetermineAttack();
        /*        }
                else
                    print("Character is dead, you shouldn't reach here, something went wrong");*/
    }
    AttackBase DetermineAttackFromList(AttackBase selectedAttack = null)
    {
        AttackBase attackToUse;
        if (selectedAttack == null) {
            attackToUse = attacks[Random.Range(0, attacks.Count - 1)];
        } else {
            attackToUse = selectedAttack;
        }
        Debug.Log($"{name} is casting attack {attackToUse.name}");
        return attackToUse;
    }
    Unit SelectAutoRandomTarget()
    {
        GameObject randomTarget = battle.GetRandomEnemy();
        if (randomTarget == null) {
            Debug.Log($"Unable to select random target.");
        }
        if (isAPlayer)
        {
            return randomTarget.GetComponent<Unit>();
        }
        else
        { 
            return randomTarget.GetComponent<Unit>();
        }
    }

    private bool DidNotFlee() {
        int fleeThisTurn = Random.Range(100, 100);

        if (fleeThisTurn >= 50 && canFlee) {
            print("Character Fled");
            experienceEarned = 0;
            CurrentHP = 0;

/*            affinityText.color = Color.black;
            affinityText.text = "Fled";
            StartCoroutine(ClearText());*/

            Instantiate(deathParticle, transform.position, transform.rotation);
            currentState = UnitState.Fled;
            battle.ExperienceAndDeathCollection();
            //character fled

            Debug.LogWarning("Fled");
            return false;
        }
        return true;
    }

    public void TakeDamage(float damageToTake)
    {
        CurrentHP -= damageToTake;

        if(CurrentHP <= 0) 
        {
            UnitDeath();
        }
    }

    void UnitDeath()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
        this.gameObject.SetActive(false);

        if(isAPlayer)
        {
            currentState = UnitState.Unconscious;
        }
        else
        {
            battle.ExperienceAndDeathCollection();
            battle.Combatants.Remove(this.gameObject);
        }
            
    }
    public bool DidAttackKillCharacter(float damageToTake, float criticalChance)
    {
        //Calculating Chance for Dodging the attack
        float chanceForAttackToLand = Random.Range(0, 100);
        float chanceForCritial = Random.Range(0, 100);

        if (chanceForAttackToLand <= (Agility + AgilityEquipment + (CurrentLevel * 1.25f)))
        {
            if (!isAPlayer)
            {
                affinityText.color = Color.white;
                affinityText.text = "Dodge";
                StartCoroutine(ClearText());
            }
            print("Dodge");
            return false;
        }
        else
        {
            //if enemy was already knocked down, add 20% damage (as if their defense was lowered)
            if (hasBeenKnockedDown)
                damageToTake *= 1.2f;
            else
                anim.SetTrigger("TakeDamage");

           /* if (charms && StrC || physical && StrP || darkArts && StrD || transfiguration && StrT || ancient && StrA)
            {
                damageToTake = damageToTake / 2;

                if (!isAPlayer)
                {
                    affinityText.color = Color.black;
                    affinityText.text = "Resist";
                    StartCoroutine(ClearText());
                }
            }

            else if (charms && WeakC || physical && WeakP || darkArts && WeakD || transfiguration && WeakT || ancient && WeakA)
            {
                damageToTake = damageToTake * 2;

                if (!isAPlayer)
                {
                    affinityText.color = Color.yellow;
                    affinityText.text = "Weak";
                    hasBeenKnockedDown = true;
                    anim.SetBool("knockedDown", true);
                    StartCoroutine(ClearText());
                }
                if (isAPlayer)
                {
                    anim.SetBool("knockedDown", true);
                }
                battle.CheckIfAllMembersKnockedDown();
            }

            else if(chanceForCritial <= criticalChance)
            {
                damageToTake = damageToTake * 2;

                if (!isAPlayer)
                {
                    affinityText.color = Color.red;
                    affinityText.text = "Critical";
                    StartCoroutine(ClearText());
                }
                print($"{name} was knocked down. ");
                battle.CheckIfAllMembersKnockedDown();
            }*/

            if (isMetallic)
                damageToTake = 1;

            CurrentHP -= (damageToTake * defenseMultiplier);
            Instantiate(battle.damageParticle, transform.position, transform.rotation);

            if (!isAPlayer)
                healthBar.value = CurrentHP / MaxHP;
            else
                battle.UpdatePlayerHealthManaUI();

            if (CurrentHP >= 0)
            {
                return false;
            }
            else
            {
                //  anim.SetTrigger("isDead");
                Instantiate(deathParticle, transform.position, transform.rotation);
                currentState = UnitState.Unconscious;
                battle.ExperienceAndDeathCollection();
                return true;
            }         
        }
    }

    public void RotateCamera()
    {
        battle.RotateCamera();
    }

    public void AdvanceTurn()
    {
        battle.AdvanceTurn();
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(1.5f);
        affinityText.text = "";
    }

    public void CleanUp()
    {
        isASoloAttack = false;
        isAGroupAttack = false;
        isAHeal = false;

        // TODO : Resolve current status effects

        charms = false;
        physical = false;
        darkArts = false;
        transfiguration = false;
        ancient = false;

        deductCost = 0;
        damage = 0;
        healthToRecover = 0;
        tempAgility = 0;
    }
}
