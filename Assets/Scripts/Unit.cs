using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //HP = Health, MP = Magic, Magic = damage through magic, Physical = damage through physical, agility = ability to dodge, finesse = critical chance
    public bool isOnAuto = true;
    public bool isAPlayer;

    public string Name;
    public float CurrentLevel = 1, CurrentHP = 25, MaxHP = 20, CurrentMP = 30, MaxMP = 25, Magic = 10, Physical = 9, Agility = 15, Finesse = 20;
    public float MagicEquipment, PhysicalEquipment, AgilityEquipment, FinesseEquipment;

    public bool StrT, StrC, StrP, StrD, StrA;
    public bool WeakT, WeakC, WeakP, WeakD, WeakA;

    public bool essenceOfPride, pestectus, pillarOfStrength, potionOfHealing, potionofResolve, potionOfResurrection;

    float lightAttack = 1f, mediumAttack = 1.4f;

    public bool isASoloAttack, isAGroupAttack, isAHeal;
    bool charms, physical, darkArts, transfiguration, ancient;
    bool attackBoosted, defenseBoosted, charged;

    public int attackBonusTurnCount, defenseBonusTurnCount;

    public float attackMultiplier = 1, defenseMultiplier = 1;

    public float damage, healthToRecover;

    public bool characterIsDead;
    public float experienceEarned;

    Battle battle;

    public List<string> KnownSkills = new List<string>();

    private void Start()
    {
        battle = GameObject.Find("Battle").GetComponent<Battle>();
    }

    public void TakingUnitTurn()
    {
        if (!characterIsDead)
        {
            if (attackBonusTurnCount > 0)
                attackBonusTurnCount--;
            else
                attackMultiplier = 1;

            if (defenseBonusTurnCount > 0)
                defenseBonusTurnCount--;
            else
                defenseMultiplier = 1;


            if (isOnAuto)
            {
                int randomAttack = Random.Range(0, KnownSkills.Count);
                Invoke(KnownSkills[randomAttack], 0f);
                print(KnownSkills[randomAttack]);
            }
        }
    }

    //Basic Calculation for attack: 
    //Example: damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * lightAttack;
    //Breakdown: damage = {[ Square root of 'MagicEquipment' bonus * Square root of 'Magic' stat ] add (CurrentLevel * 25%) + Square root of 'Physical' stat } * Attack Multiplier from bonuses * Variance between 95-110% * LightAttack modifier
    // this was based on Persona 5's calculation of damage = [ Square Root of Skill Power * Square Root of Stat ] 

    #region Full List of Attacks
    public void BasicCast()
    {
        isASoloAttack = true;
        physical = true;

        //This attack costs nothing so it is only 60% base damage
        damage = (((Mathf.Sqrt(PhysicalEquipment) * Mathf.Sqrt(Physical)) + (CurrentLevel * 1.25f)) * attackMultiplier * Random.Range(.95f, 1.1f)) * .6f;

        battle.ResolvingATurn();
    }

    public void Ajei()
    {
        isASoloAttack = true;
        ancient = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * lightAttack;

        battle.ResolvingATurn();
    }

    public void Deeza()
    {
        isASoloAttack = true;
        ancient = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * mediumAttack;

        battle.ResolvingATurn();
    }

    public void Hookeeghan()
    {
        isAGroupAttack = true;
        ancient = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * mediumAttack;

        battle.ResolvingATurn();
    }

    public void AuraDEau()
    {
        isASoloAttack = true;
        charms = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * lightAttack;

        battle.ResolvingATurn();
    }

    public void CriDeGivre()
    {
        isASoloAttack = true;
        charms = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * mediumAttack;

        battle.ResolvingATurn();
    }

    public void FuseeDeVent()
    {
        isAGroupAttack = true;
        charms = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * mediumAttack;

        battle.ResolvingATurn();
    }

    public void HomdomIritus()
    {
        isAGroupAttack = true;
        darkArts = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * lightAttack;

        battle.ResolvingATurn();
    }

    public void ManictomDotum()
    {
        isASoloAttack = true;
        darkArts = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * lightAttack;

        battle.ResolvingATurn();
    }

    public void OrbemPardonti()
    {
        isASoloAttack = true;
        darkArts = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * mediumAttack;

        battle.ResolvingATurn();
    }

    public void ScoptumTalli()
    {
        isAGroupAttack = true;
        darkArts = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * mediumAttack;
        CurrentHP = 1;

        battle.ResolvingATurn();
    }

    public void Charge()
    {
        isAHeal = true;

        //this is only for 1 turn, but the way the calcualtion is done, the turn will rollover from 2->1, the player will have a turn, then the next turn will be 1->0 which is normal
        attackBonusTurnCount = 2;
        attackMultiplier = 2;

        battle.ResolvingATurn();
    }

    public void EssenceOfPride()
    {
        isAHeal = true;
        essenceOfPride = true;

        battle.ResolvingATurn();
    }

    public void Pestectus()
    {
        isAHeal = true;
        pestectus = true;

        healthToRecover = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * .25f;

        battle.ResolvingATurn();
    }

    public void PillarOfStrength()
    {
        pillarOfStrength = true;
        isAHeal = true;

        battle.ResolvingATurn();
    }

    public void PotionOfHealing()
    {
        potionOfHealing = true;
        isAHeal = true;

        healthToRecover = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * .25f;

        battle.ResolvingATurn();
    }

    public void PotionOfResolve()
    {
        potionofResolve = true;
        isAHeal = true;

        battle.ResolvingATurn();
    }

    public void PotionOfResurrection()
    {
        potionOfResurrection = true;
        isAHeal = true;

        battle.ResolvingATurn();
    }
    public void ForcefulLunge()
    {
        isASoloAttack = true;
        physical = true;

        damage = ((Mathf.Sqrt(PhysicalEquipment) * Mathf.Sqrt(Physical)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Magic))) * attackMultiplier * Random.Range(.95f, 1.1f) * lightAttack;

        battle.ResolvingATurn();
    }
    public void PlayfulShove()
    {
        isAGroupAttack = true;
        physical = true;

        damage = ((Mathf.Sqrt(PhysicalEquipment) * Mathf.Sqrt(Physical)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Magic))) * attackMultiplier * Random.Range(.95f, 1.1f) * mediumAttack;

        battle.ResolvingATurn();
    }

    public void VerticalSlice()
    {
        isASoloAttack = true;
        physical = true;

        damage = ((Mathf.Sqrt(PhysicalEquipment) * Mathf.Sqrt(Physical)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Magic))) * attackMultiplier * Random.Range(.95f, 1.1f) * lightAttack;

        battle.ResolvingATurn();
    }

    public void ExpelindoElemicus()
    {
        isASoloAttack = true;
        transfiguration = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * lightAttack;

        battle.ResolvingATurn();
    }

    public void FierelloDeflectus()
    {
        isASoloAttack = true;
        transfiguration = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * mediumAttack;

        battle.ResolvingATurn();
    }

    public void SellamTrahere()
    {
        isAGroupAttack = true;
        transfiguration = true;

        damage = ((Mathf.Sqrt(MagicEquipment) * Mathf.Sqrt(Magic)) + ((CurrentLevel * 1.25f) + Mathf.Sqrt(Physical))) * attackMultiplier * Random.Range(.95f, 1.1f) * lightAttack;

        battle.ResolvingATurn();
    }
    #endregion

    public bool DidAttackKillCharacter(float damageToTake, float criticalChance)
    {
        damage = damageToTake;
        //Calculating Chance for Dodging the attack
        float chanceForAttackToLand = Random.Range(0, 100);
        float chanceForCritial = Random.Range(0, 100);

        if (chanceForAttackToLand <= (Agility + AgilityEquipment + (CurrentLevel * 1.25f)))
        {
            print("Attack Dodged");
            return false;
        }
        else
        {
            if (charms && StrC || physical && StrP || darkArts && StrD || transfiguration && StrT || ancient && StrA)
            {
                damage = damage / 2;
                print("Resist!");
            }

            else if (charms && WeakC || physical && WeakP || darkArts && WeakD || transfiguration && WeakT || ancient && WeakA)
            {
                damage = damage * 2;
                print("Weak!");
                print("Enemy Knocked Down");
            }

            else if(chanceForCritial <= criticalChance)
            {
                damage = damage * 2;
                print("Critical!");
                print("Enemy Knocked Down");
            }

            CurrentHP -= (damage * defenseMultiplier);

            if (CurrentHP >= 0)
            {
                return false;
            }
            else
            {
                characterIsDead = true;
                battle.ExperienceAndDeathCollection();
                return true;
            }         
        }
    }

    public void CleanUp()
    {
        isASoloAttack = false;
        isAGroupAttack = false;
        isAHeal = false;

        essenceOfPride = false;
        pestectus = false;
        pillarOfStrength = false;
        potionOfHealing = false;
        potionofResolve = false;
        potionOfResurrection = false;

        charms = false;
        physical = false;
        darkArts = false;
        transfiguration = false;
        ancient = false;

        damage = 0;
        healthToRecover = 0;
    }
}
