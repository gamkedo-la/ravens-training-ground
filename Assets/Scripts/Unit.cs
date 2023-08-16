using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Character.Stats;
using System.Linq;

public class Unit : MonoBehaviour
{
    //HP = Health, MP = Magic, Magic = damage through magic, Physical = damage through physical, agility = ability to dodge, finesse = critical chance
    //public Health health;

    [SerializeField]
    AIBrain aIBrain;
    public StationController stationController;
    public bool isOnAuto = true;
    public bool isAPlayer;

    public string Name;
    public float CurrentMP = 30;
    public float startingMagic = 10, startingPhysical = 9, startingAgility = 15, startingFinesse = 20;
    public int CurrentLevel = 1;

    public float MagicEquipment, PhysicalEquipment, AgilityEquipment, FinesseEquipment;
    public Unit leftUnit, rightUnit;
    public List<Affinity> resistances;
    public List<Affinity> weaknesses;
    public List<Enhancement> enhancements;
    //float lightAttack = 1f, mediumAttack = 1.4f;

    public bool isASoloAttack, isAGroupAttack, isAHeal;
/*    bool charms, physical, darkArts, transfiguration, ancient;
    bool attackBoosted, defenseBoosted, charged;*/

    public int attackBonusTurnCount, defenseBonusTurnCount;

    public float attackMultiplier = 1, defenseMultiplier = 1;

    public float healthToRecover;

    public float experienceEarned;

    public bool hasBeenKnockedDown;

    public Animator anim;
    public GameObject powerUpParticle, deathParticle, targetParticle, canvas;

    //Cost
    /*int deductCost;
    public int tier0 = 0, tier1 = 4, tier2 = 8, tier3 = 12, tier4 = 15, tier5 = 18, tier6 = 21;*/

    Battle battle;

    public List<AbilityBase> abilities;

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

    List<Unit> targets;

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
    private BaseStats baseStats;


    private void Start()
    {
        if (aIBrain == null)
            aIBrain=GetComponent<AIBrain>();

        currentState = UnitState.IDLE;
        baseStats = GetComponent<BaseStats>();
        battle = GameObject.Find("Battle").GetComponent<Battle>();
        GetComponent<Health>().OnDie += UnitDeath;

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
                /*
                 * TODO: Add enhancement with metallic properties
                    CurrentHP = 5;
                    MaxHP = 5;
                    Agility = 80;
                    Finesse = 80;
                */
                experienceEarned *= 5; 
                canFlee = true;
            }
        }
        foreach(Enhancement enhancement in enhancements)
        {
            enhancement.Initialize(this);
        }
    }
    private void OnValidate()
    {
        
    }

    private void Update()
    {

    }

    IEnumerator WaitingMoment()
    {
        yield return new WaitForEndOfFrame();

    }
    public List<Unit> GetNeighborUnits()
    {
        return stationController.GetNeighborUnits();
    }

    public IEnumerator TakingUnitTurn(AbilityBase selectedAbility = null)
    {
        //break out if unit is unconcious dead or fled
        if (currentState == UnitState.Unconscious)
        { yield return new WaitForEndOfFrame(); }
        if (currentState == UnitState.Fled)
        { yield return new WaitForEndOfFrame(); }
        if (currentState == UnitState.Dead)
        { yield return new WaitForEndOfFrame(); }
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
            yield return new WaitForEndOfFrame();

        AbilityBase abilityBaseTemp = DetermineAbilityFromList(selectedAbility);

       
        if (isOnAuto)
        {

            targets = aIBrain.SelectTarget(abilityBaseTemp, this, battle.Combatants.Select(r => r.GetComponent<Unit>()).Where(g => g != null).ToList());

            if (abilityBaseTemp.AttemptAbility(this, targets))
            {
                anim.SetTrigger("Attack");
                //this waits the duration of the attack (1.2 seconds), but only 75% through the animation before playing 'takedamage'
                Invoke("WaitingForAttackToHit", 1.2f * .75f);
            }
        }

        yield return new WaitForSeconds(1);
        // DetermineAttack();
        /*        }
                else
                    print("Character is dead, you shouldn't reach here, something went wrong");*/
    }

    public void WaitingForAttackToHit()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].anim.SetTrigger("TakeDamage");
        }
    }


    public IEnumerator UpdateUI()
    {
        yield return new WaitForSeconds(1.25f);
        if (!isAPlayer)
        {
            healthBar.maxValue = gameObject.GetComponent<Health>().startingHitPoints;
            healthBar.value = gameObject.GetComponent<Health>().GetCurrentHP();
            print(gameObject.GetComponent<Health>().GetCurrentHP() + " / " + gameObject.GetComponent<Health>().startingHitPoints);
            print(healthBar.value);
        }
    }

    public IEnumerator TurnOffUI()
    {
        yield return new WaitForSeconds(2.5f);
        //coming from Health.cs if it is an enemy
        //once the turn is over, turn off their UI
        canvas.SetActive(false);
    }
    AbilityBase DetermineAbilityFromList(AbilityBase selectedAbility = null)
    {
        AbilityBase abilityToUse;
        if (selectedAbility != null)
        {
            abilityToUse = selectedAbility;
            return abilityToUse;
        }

        abilityToUse = aIBrain.SelectAbility(abilities);
        //Debug.Log($"{name} is casting attack {abilityToUse.name}");
        return abilityToUse;
    }
/*    Unit SelectAutoRandomTarget()
    {
        GameObject randomTarget = battle.GetRandomEnemy(this);
        if (randomTarget == null)
        {
            Debug.Log($"Unable to select random target.");
        }

        return randomTarget.GetComponent<Unit>();
    }*/

    private bool DidNotFlee() {
        int fleeThisTurn = Random.Range(100, 100);

        if (fleeThisTurn >= 50 && canFlee) {
            print("Character Fled");
            experienceEarned = 0;
            GetComponent<Health>().Die();

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

    public void Heal(float healValue)
    {
        GetComponent<Health>().AddHealth((int) healValue);
    }
    public void AddEnhancement(Enhancement enhancement)
    {
        enhancement.Initialize(this);
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
        print("DID WE HIT THIS");
        //Calculating Chance for Dodging the attack
        float chanceForAttackToLand = Random.Range(0, 100);
        float chanceForCritial = Random.Range(0, 100);

        if (chanceForAttackToLand <= (GetComponent<BaseStats>().GetStat(Stat.Agility) + AgilityEquipment + (CurrentLevel * 1.25f)))
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

            //GetComponent<Health>().TakeDamage(this.gameObject, (int) (damageToTake * defenseMultiplier));
            Instantiate(battle.damageParticle, transform.position, transform.rotation);

            if (!isAPlayer)
                healthBar.value = GetComponent<Health>().GetCurrentHP() / GetComponent<Health>().GetMaxHP();
            else
                battle.UpdatePlayerHealthManaUI();

            if (GetComponent<Health>().GetCurrentHP() >= 0)
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
        //battle.RotateCamera();
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
/*
        charms = false;
        physical = false;
        darkArts = false;
        transfiguration = false;
        ancient = false;
*/
        //deductCost = 0;
        //damage = 0;
        healthToRecover = 0;
        //tempAgility = 0;
    }
}
