using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public enum StateOfBattle { Start, PC1, PC2, PC3, PC4, Party, EnemyTurn, WON, LOST }

public class Battle : MonoBehaviour
{
    public StateOfBattle state;

    public Transform BattleStation1, BattleStation2, BattleStation3, BattleStation4;
    public Transform enemyBattleStation1, enemyBattleStation2, enemyBattleStation3, enemyBattleStation4;

    GameObject currentParty0, currentParty1, currentParty2, currentParty3;
    GameObject currentEnemy0, currentEnemy1, currentEnemy2, currentEnemy3;
    public Vector3 platformOffset = new Vector3(0, 1, 0);

    public bool floor1 = true, floor2, floor3;
    public List<string> enemiesFloor1 = new List<string>();
    public List<string> enemiesFloor2 = new List<string>();
    public List<string> enemiesFloor3 = new List<string>();

    public List<string> enemiesInThisFight = new List<string>();
    public int enemyCount;

    public List<GameObject> Combatants = new List<GameObject>();
    public List<GameObject> playersInThisFight = new List<GameObject>();
    public List<GameObject> enemiesInFight = new List<GameObject>();

    float totalExperienceAwarded;
    int currentCombatant;
    int deadPlayers;

    void Start()
    {
        if (floor1)
        {
            int percent = Random.Range(1, 100);
            if (percent < 30)
                enemyCount = 1;
            else if (percent >= 30 && percent < 60)
                enemyCount = 2;
            else if (percent >= 60 && percent < 89)
                enemyCount = 3;
            else 
                enemyCount = 4;

            for (int i = 0; i < enemyCount; i++)
            {
                int nameToChoose = Random.Range(0, enemiesFloor1.Count);
                string currentNameChosen = enemiesFloor1[nameToChoose];
                enemiesInThisFight.Add(currentNameChosen);
            }
            
        }
        StartCoroutine(SetUpBattle());
    }
    IEnumerator SetUpBattle()
    {
        yield return new WaitForSeconds(0.25f);

        #region This Adds members to the Party as well as Sorts by Agility
        currentParty0 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[0]), BattleStation1.transform.position + platformOffset, BattleStation1.transform.rotation);
        Combatants.Add(currentParty0);
        playersInThisFight.Add(currentParty0);

        if (GameManager.inCurrentParty.Count > 0)
        {
            currentParty1 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[1]), BattleStation2.transform.position + platformOffset, BattleStation2.transform.rotation);
            Combatants.Add(currentParty1);
            playersInThisFight.Add(currentParty1);
        }
        if (GameManager.inCurrentParty.Count > 1)
        {
            currentParty2 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[2]), BattleStation3.transform.position + platformOffset, BattleStation3.transform.rotation);
            Combatants.Add(currentParty2);
            playersInThisFight.Add(currentParty2);
        }

        if (GameManager.inCurrentParty.Count > 2)
        {
            currentParty3 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[3]), BattleStation4.transform.position + platformOffset, BattleStation4.transform.rotation);
            Combatants.Add(currentParty3);
            playersInThisFight.Add(currentParty3);
        }

        currentEnemy0 = Instantiate(Resources.Load<GameObject>(enemiesInThisFight[0]), enemyBattleStation1.transform.position + platformOffset, enemyBattleStation1.transform.rotation);
        Combatants.Add(currentEnemy0);
        enemiesInFight.Add(currentEnemy0);

        if (enemyCount > 1)
        {
            currentEnemy1 = Instantiate(Resources.Load<GameObject>(enemiesInThisFight[1]), enemyBattleStation2.transform.position + platformOffset, enemyBattleStation2.transform.rotation);
            Combatants.Add(currentEnemy1);
            enemiesInFight.Add(currentEnemy1);
        }
      
        if (enemyCount > 2)
        { 
            currentEnemy2 = Instantiate(Resources.Load<GameObject>(enemiesInThisFight[2]), enemyBattleStation3.transform.position + platformOffset, enemyBattleStation3.transform.rotation);
            Combatants.Add(currentEnemy2);
            enemiesInFight.Add(currentEnemy2);
        }
        if (enemyCount > 3)
        {
            currentEnemy3 = Instantiate(Resources.Load<GameObject>(enemiesInThisFight[3]), enemyBattleStation4.transform.position + platformOffset, enemyBattleStation4.transform.rotation);
            Combatants.Add(currentEnemy3);
            enemiesInFight.Add(currentEnemy3);
        }

        Combatants = Combatants.OrderByDescending(x => x.GetComponent<Unit>().Agility).ToList();
        #endregion

        NextTurn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentCombatant++;
            NextTurn();
        }
    }

    void NextTurn()
    {
        if (currentCombatant >= Combatants.Count)
            currentCombatant = 0;

        print("Currently Up: " + Combatants[currentCombatant].GetComponent<Unit>().Name + " Agility: " + Combatants[currentCombatant].GetComponent<Unit>().Agility + " CurrentHP: " + Combatants[currentCombatant].GetComponent<Unit>().CurrentHP);
        Combatants[currentCombatant].GetComponent<Unit>().TakingUnitTurn();
    }

    public void ResolvingATurn()
    {
        if (Combatants[currentCombatant].GetComponent<Unit>().isAPlayer)
        {
            if (Combatants[currentCombatant].GetComponent<Unit>().isASoloAttack)
            {
                enemiesInFight = enemiesInFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();
                int randAttack = Random.Range(0, 100);
                if (randAttack < 50)
                {
                    enemiesInFight[0].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
                }
                else
                {
                    int opponentToAttack = Random.Range(0, enemiesInFight.Count);
                    enemiesInFight[opponentToAttack].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
                }
            }

            else if (Combatants[currentCombatant].GetComponent<Unit>().isAGroupAttack)
            {
                for (int i = 0; i < enemiesInFight.Count; i++)
                {
                    enemiesInFight[i].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
                }
            }

            else if (Combatants[currentCombatant].GetComponent<Unit>().isAHeal)
            {
                if (Combatants[currentCombatant].GetComponent<Unit>().essenceOfPride)
                {
                    for (int i = 0; i < playersInThisFight.Count; i++)
                    {
                        playersInThisFight[i].GetComponent<Unit>().attackMultiplier = 1.5f;
                        playersInThisFight[i].GetComponent<Unit>().defenseMultiplier = .5f;

                        playersInThisFight[i].GetComponent<Unit>().attackBonusTurnCount = 3;
                        playersInThisFight[i].GetComponent<Unit>().defenseBonusTurnCount = 3;
                    }
                }
                else if(Combatants[currentCombatant].GetComponent<Unit>().pestectus)
                {
                    for (int i = 0; i < playersInThisFight.Count; i++)
                    {
                        playersInThisFight[i].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (playersInThisFight[i].GetComponent<Unit>().CurrentHP >= playersInThisFight[i].GetComponent<Unit>().MaxHP)
                        {
                            playersInThisFight[i].GetComponent<Unit>().CurrentHP = playersInThisFight[i].GetComponent<Unit>().MaxHP;
                        }
                    }
                }
                else if (Combatants[currentCombatant].GetComponent<Unit>().pillarOfStrength)
                {
                    Combatants[currentCombatant].GetComponent<Unit>().defenseMultiplier = .5f;
                    Combatants[currentCombatant].GetComponent<Unit>().defenseBonusTurnCount = 3;
                }
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionOfHealing)
                {
                    playersInThisFight = playersInThisFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();

                    int chanceOfPersonToHeal = Random.Range(0, 100);
                    if (chanceOfPersonToHeal <= 80)
                    {
                        playersInThisFight[0].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (playersInThisFight[0].GetComponent<Unit>().CurrentHP >= playersInThisFight[0].GetComponent<Unit>().MaxHP)
                        {
                            playersInThisFight[0].GetComponent<Unit>().CurrentHP = playersInThisFight[0].GetComponent<Unit>().MaxHP;
                        }
                    }
                    else
                    {
                        int personToHeal = Random.Range(0, playersInThisFight.Count);
                        playersInThisFight[personToHeal].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (playersInThisFight[personToHeal].GetComponent<Unit>().CurrentHP >= playersInThisFight[personToHeal].GetComponent<Unit>().MaxHP)
                        {
                            playersInThisFight[personToHeal].GetComponent<Unit>().CurrentHP = playersInThisFight[personToHeal].GetComponent<Unit>().MaxHP;
                        }
                    }
                }
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionofResolve)
                {
                    Combatants[currentCombatant].GetComponent<Unit>().attackMultiplier = 1.5f;
                    Combatants[currentCombatant].GetComponent<Unit>().attackBonusTurnCount = 3;
                }
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionOfResurrection)
                {
                    //Come back to this
                }
            }
        }
        
        else
        {
            if (Combatants[currentCombatant].GetComponent<Unit>().isASoloAttack)
            {
                playersInThisFight = playersInThisFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();

                int playerToAttack = Random.Range(0, playersInThisFight.Count);
                playersInThisFight[playerToAttack].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
            }

            else if (Combatants[currentCombatant].GetComponent<Unit>().isAGroupAttack)
            {
                for (int i = 0; i < playersInThisFight.Count; i++)
                {
                    playersInThisFight[i].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
                }
            }

            else if (Combatants[currentCombatant].GetComponent<Unit>().isAHeal)
            {
                if (Combatants[currentCombatant].GetComponent<Unit>().essenceOfPride)
                {
                    for (int i = 0; i < enemiesInFight.Count; i++)
                    {
                        enemiesInFight[i].GetComponent<Unit>().attackMultiplier = 1.5f;
                        enemiesInFight[i].GetComponent<Unit>().defenseMultiplier = .5f;

                        enemiesInFight[i].GetComponent<Unit>().attackBonusTurnCount = 3;
                        enemiesInFight[i].GetComponent<Unit>().defenseBonusTurnCount = 3;
                    }
                }
                else if (Combatants[currentCombatant].GetComponent<Unit>().pestectus)
                {
                    for (int i = 0; i < enemiesInFight.Count; i++)
                    {
                        enemiesInFight[i].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (enemiesInFight[i].GetComponent<Unit>().CurrentHP >= enemiesInFight[i].GetComponent<Unit>().MaxHP)
                        {
                            enemiesInFight[i].GetComponent<Unit>().CurrentHP = enemiesInFight[i].GetComponent<Unit>().MaxHP;
                        }
                    }
                }
                else if (Combatants[currentCombatant].GetComponent<Unit>().pillarOfStrength)
                {
                    Combatants[currentCombatant].GetComponent<Unit>().defenseMultiplier = .5f;
                    Combatants[currentCombatant].GetComponent<Unit>().defenseBonusTurnCount = 3;
                }
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionOfHealing)
                {
                    enemiesInFight = enemiesInFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();

                    int chanceOfPersonToHeal = Random.Range(0, 100);
                    if (chanceOfPersonToHeal <= 80)
                    {
                        enemiesInFight[0].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (enemiesInFight[0].GetComponent<Unit>().CurrentHP >= enemiesInFight[0].GetComponent<Unit>().MaxHP)
                        {
                            enemiesInFight[0].GetComponent<Unit>().CurrentHP = enemiesInFight[0].GetComponent<Unit>().MaxHP;
                        }
                    }
                    else
                    {
                        int enemyToHeal = Random.Range(0, enemiesInFight.Count);
                        enemiesInFight[enemyToHeal].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (enemiesInFight[enemyToHeal].GetComponent<Unit>().CurrentHP >= enemiesInFight[enemyToHeal].GetComponent<Unit>().MaxHP)
                        {
                            enemiesInFight[enemyToHeal].GetComponent<Unit>().CurrentHP = enemiesInFight[enemyToHeal].GetComponent<Unit>().MaxHP;
                        }
                    }
                }
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionofResolve)
                {
                    Combatants[currentCombatant].GetComponent<Unit>().attackMultiplier = 1.5f;
                    Combatants[currentCombatant].GetComponent<Unit>().attackBonusTurnCount = 3;
                }
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionOfResurrection)
                {
                    //Come back to this
                }
            }
        }

        for (int i = 0; i < Combatants.Count; i++)
        {
            Combatants[i].GetComponent<Unit>().CleanUp();
        }
    }

    public void ExperienceAndDeathCollection()
    {
        for (int i = 0; i < Combatants.Count; i++)
        {
            if (Combatants[i].GetComponent<Unit>().CurrentHP <= 0)
            {
                totalExperienceAwarded += Combatants[i].GetComponent<Unit>().experienceEarned;

                foreach (Transform child in Combatants[i].transform)
                {
                    child.gameObject.SetActive(false);
                }
                Combatants.Remove(Combatants[i]);
            }
        }

        for (int i = 0; i < enemiesInFight.Count; i++)
        {
            if (enemiesInFight[i].GetComponent<Unit>().CurrentHP <= 0)
            {
                totalExperienceAwarded += Combatants[i].GetComponent<Unit>().experienceEarned;
                enemiesInFight.Remove(enemiesInFight[i]);
            }
        }

        if (enemiesInFight.Count <= 0)
        {
            state = StateOfBattle.WON;
            EndBattle();
        }

        for (int i = 0; i < playersInThisFight.Count; i++)
        {
            if (playersInThisFight[i].GetComponent<Unit>().characterIsDead)
            {
                deadPlayers++;
            }
        }

        if (deadPlayers >= playersInThisFight.Count)
        {
            state = StateOfBattle.LOST;
            EndBattle();
        }
    }

    void EndBattle()
    {
        if (state == StateOfBattle.LOST)
        {
            print("Player Lost");
        }
        else
        {
            print("Player Won");
        }
    }

    public void EndTurn()
    {
        currentCombatant++;
        NextTurn();
    }
}
