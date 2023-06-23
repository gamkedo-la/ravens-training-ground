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

    List<GameObject> deadCharacters = new List<GameObject>();

    float totalExperienceAwarded;
    int currentCombatant, deadPlayers;

    int enemyCountForKnockedOut, playerCountForKnockedOut;

    //UI for Players
    public List<Slider> healthUI = new List<Slider>();
    public List<Slider> manaUI = new List<Slider>();
    public List<TMP_Text> healthText = new List<TMP_Text>();
    public List<TMP_Text> manaText = new List<TMP_Text>();
    public List<Image> icons = new List<Image>();
    public List<Image> triangles = new List<Image>();

    public GameObject powerUpParticle;
    int playerToAttack;

    //REMINDER ACTION 'FLEE' needs to have the 'ClearGameManager() function to get rid of carryover data

    void Start()
    {
        if (GameManager.enemyCount > 0)
        {
            enemyCount = GameManager.enemyCount;

            for (int i = 0; i < enemyCount; i++)
            {
                string currentNameChosen = GameManager.enemiesInThisFight[i];
                enemiesInThisFight.Add(currentNameChosen);
            }
        }
        else
        {
            //if player is on floor 1, this determines the percentage of chance for a certain number of enemies
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
                //floor 2 will go here
                //floor 3 will contain 1 enemy - the final boss
            }
        }

        StartCoroutine(SetUpBattle());
    }
    IEnumerator SetUpBattle()
    {
        yield return new WaitForSeconds(0.25f);

        //This segment add players to the player list, enemies to the enemy list, and players/enemies to a combined 'combatants' list
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

        UpdatePlayerHealthManaUI();
        NextTurn();
    }

    private void Update()
    {
        //Pressing Space advances the turn - this is not permenant - just for testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentCombatant++;
            NextTurn();
        }
    }

    void NextTurn()
    {
        //The combatants list is sorted in order from largest to smallest incrementing by one. 
        //When the list hits greater than the number of combatants, it re-sorts the list (trying to capture any agility changes in the turn) then circles back around to the largest
        if (currentCombatant >= Combatants.Count)
        {
            Combatants = Combatants.OrderByDescending(x => x.GetComponent<Unit>().Agility).ToList();
            currentCombatant = 0;
        }


        print("Currently Up: " + Combatants[currentCombatant].GetComponent<Unit>().Name + " Agility: " + Combatants[currentCombatant].GetComponent<Unit>().Agility + " CurrentHP: " + Combatants[currentCombatant].GetComponent<Unit>().CurrentHP);
        Combatants[currentCombatant].GetComponent<Unit>().TakingUnitTurn();
    }

    public void ResolvingATurn()
    {
        //The entire turn system occurs in here
        //Currently this is all reliant on the pseudo automated system of pressing space
        //The bones of this system will be used later for a 'Tactics' section as well as offering low level enemy AI options 
        #region Player Turn
        Combatants[currentCombatant].GetComponent<Unit>().anim.SetTrigger("Attack");

        if (Combatants[currentCombatant].GetComponent<Unit>().isAPlayer)
        {

            //During a solo attack, the automated player has a 50% chance to hit the lowest HP target, then a 50% chance to randomly hit any other enemy (including the lowest)
            if (Combatants[currentCombatant].GetComponent<Unit>().isASoloAttack)
            {         
                enemiesInFight = enemiesInFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();
                int randAttack = Random.Range(0, 100);
                if (randAttack < 50)
                {
                    Combatants[currentCombatant].transform.LookAt(enemiesInFight[0].transform);
                    enemiesInFight[0].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
                }
                else
                {
                    int opponentToAttack = Random.Range(0, enemiesInFight.Count);
                    enemiesInFight[opponentToAttack].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
                    Combatants[currentCombatant].transform.LookAt(enemiesInFight[opponentToAttack].transform);
                }
            }

            //Group attacks hit every enemy at the same time
            else if (Combatants[currentCombatant].GetComponent<Unit>().isAGroupAttack)
            {
                for (int i = 0; i < enemiesInFight.Count; i++)
                {
                    enemiesInFight[i].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
                }
            }

            //This section talks about the different support 'attacks' [isAHeal is derived from 'Unit']
            else if (Combatants[currentCombatant].GetComponent<Unit>().isAHeal)
            {
                //Essence Of Pride increases the party's attack by 50%, defense by 50%, and keeps those stats for 3 turns [this is hard coded to prevent stacking = 3 is different than += 3]
                if (Combatants[currentCombatant].GetComponent<Unit>().essenceOfPride)
                {
                    for (int i = 0; i < playersInThisFight.Count; i++)
                    {
                        playersInThisFight[i].GetComponent<Unit>().anim.SetTrigger("Charge");
                        Instantiate(powerUpParticle, playersInThisFight[i].transform.position, playersInThisFight[i].transform.rotation);
                        playersInThisFight[i].GetComponent<Unit>().attackMultiplier = 1.5f;
                        playersInThisFight[i].GetComponent<Unit>().defenseMultiplier = .5f;

                        playersInThisFight[i].GetComponent<Unit>().attackBonusTurnCount = 3;
                        playersInThisFight[i].GetComponent<Unit>().defenseBonusTurnCount = 3;
                    }
                }

                //Pestecus this slightly increases the health of all party members (that are alive)
                else if(Combatants[currentCombatant].GetComponent<Unit>().pestectus)
                {
                    for (int i = 0; i < playersInThisFight.Count; i++)
                    {
                        playersInThisFight[i].GetComponent<Unit>().anim.SetTrigger("Charge");
                        Instantiate(powerUpParticle, playersInThisFight[i].transform.position, playersInThisFight[i].transform.rotation);
                        playersInThisFight[i].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (playersInThisFight[i].GetComponent<Unit>().CurrentHP >= playersInThisFight[i].GetComponent<Unit>().MaxHP)
                        {
                            playersInThisFight[i].GetComponent<Unit>().CurrentHP = playersInThisFight[i].GetComponent<Unit>().MaxHP;
                        }
                    }
                }

                //Pillar of Strength this increases the defense of the current caster by 50% for 3 turns
                else if (Combatants[currentCombatant].GetComponent<Unit>().pillarOfStrength)
                {
                    Combatants[currentCombatant].GetComponent<Unit>().anim.SetTrigger("Charge");
                    Instantiate(powerUpParticle, Combatants[currentCombatant].transform.position, Combatants[currentCombatant].transform.rotation);
                    Combatants[currentCombatant].GetComponent<Unit>().defenseMultiplier = .5f;
                    Combatants[currentCombatant].GetComponent<Unit>().defenseBonusTurnCount = 3;
                }

                //Potion of Healing this slightly heals one party member. There is an 80% chance they heal the weakest party member, 20% chance it randomly heals another person in the party
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionOfHealing)
                {
                    playersInThisFight = playersInThisFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();

                    int chanceOfPersonToHeal = Random.Range(0, 100);
                    if (chanceOfPersonToHeal <= 80)
                    {
                        playersInThisFight[0].GetComponent<Unit>().anim.SetTrigger("Charge");
                        playersInThisFight[0].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (playersInThisFight[0].GetComponent<Unit>().CurrentHP >= playersInThisFight[0].GetComponent<Unit>().MaxHP)
                        {
                            Instantiate(powerUpParticle, playersInThisFight[0].transform.position, Combatants[currentCombatant].transform.rotation);
                            playersInThisFight[0].GetComponent<Unit>().CurrentHP = playersInThisFight[0].GetComponent<Unit>().MaxHP;
                        }
                    }
                    else
                    {
                        int personToHeal = Random.Range(0, playersInThisFight.Count);
                        playersInThisFight[personToHeal].GetComponent<Unit>().anim.SetTrigger("Charge");
                        playersInThisFight[personToHeal].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (playersInThisFight[personToHeal].GetComponent<Unit>().CurrentHP >= playersInThisFight[personToHeal].GetComponent<Unit>().MaxHP)
                        {
                            Instantiate(powerUpParticle, playersInThisFight[personToHeal].transform.position, playersInThisFight[personToHeal].transform.rotation);
                            playersInThisFight[personToHeal].GetComponent<Unit>().CurrentHP = playersInThisFight[personToHeal].GetComponent<Unit>().MaxHP;
                        }
                    }
                }

                //Potion of Resolve increases the attack of the current caster by 50% for 3 turns
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionofResolve)
                {
                    Combatants[currentCombatant].GetComponent<Unit>().anim.SetTrigger("Charge");
                    Combatants[currentCombatant].GetComponent<Unit>().attackMultiplier = 1.5f;
                    Combatants[currentCombatant].GetComponent<Unit>().attackBonusTurnCount = 3;
                }

                //Potion of Resurrection: 1)Brings one party member back to life 2) Sets that party member's health to 50% 3) adds them back into the turn order 
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionOfResurrection)
                {
                    for (int i = 0; i < playersInThisFight.Count; i++)
                    {
                        if (playersInThisFight[i].GetComponent<Unit>().characterIsDead)
                            deadCharacters.Add(playersInThisFight[i]);
                    }

                    if (deadCharacters.Count > 0)
                    {
                        Reincarnation();
                    }
                    else
                    {
                        print("No suitable target for resurrection, choose a different attack");
                        Combatants[currentCombatant].GetComponent<Unit>().RedoAttack();
                    }
                }
            }
        }
        #endregion

        #region Enemy Turn
        else
        {
            //During a solo attack, [different from player] the enemy picks a random player to attack
            if (Combatants[currentCombatant].GetComponent<Unit>().isASoloAttack)
            {
                //i think this line can go away, it is to sort player's health if we want to structure it similar to the player where they target the weakest player
                // playersInThisFight = playersInThisFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();

                for (int i = 0; i < playersInThisFight.Count; i++)
                {
                    if (!playersInThisFight[i].GetComponent<Unit>().characterIsDead)
                        playerToAttack++;
                }

                int playerToChoose = Random.Range(0, playerToAttack);
                playersInThisFight[playerToChoose].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
                Combatants[currentCombatant].transform.LookAt(playersInThisFight[playerToChoose].transform);
                playerToAttack = 0;
            }

            //This is a group attack, attacking all players (who are alive)
            else if (Combatants[currentCombatant].GetComponent<Unit>().isAGroupAttack)
            {
               for (int i = 0; i < playersInThisFight.Count; i++)
                {
                    if(!playersInThisFight[i].GetComponent<Unit>().characterIsDead)
                        playersInThisFight[i].GetComponent<Unit>().DidAttackKillCharacter(Combatants[currentCombatant].GetComponent<Unit>().damage, (Combatants[currentCombatant].GetComponent<Unit>().Finesse + Combatants[currentCombatant].GetComponent<Unit>().FinesseEquipment));
                }
            }

            //These are support 'attacks' [derived from Unit]
            else if (Combatants[currentCombatant].GetComponent<Unit>().isAHeal)
            {
                //Essence Of Pride increases the party's attack by 50%, defense by 50%, and keeps those stats for 3 turns [this is hard coded to prevent stacking = 3 is different than += 3]
                if (Combatants[currentCombatant].GetComponent<Unit>().essenceOfPride)
                {
                    for (int i = 0; i < enemiesInFight.Count; i++)
                    {
                        Instantiate(powerUpParticle, enemiesInFight[i].transform.position, enemiesInFight[i].transform.rotation);
                        enemiesInFight[i].GetComponent<Unit>().attackMultiplier = 1.5f;
                        enemiesInFight[i].GetComponent<Unit>().defenseMultiplier = .5f;

                        enemiesInFight[i].GetComponent<Unit>().attackBonusTurnCount = 3;
                        enemiesInFight[i].GetComponent<Unit>().defenseBonusTurnCount = 3;
                    }
                }

                //Pestecus this slightly increases the health of all party members (that are alive)
                else if (Combatants[currentCombatant].GetComponent<Unit>().pestectus)
                {
                    for (int i = 0; i < enemiesInFight.Count; i++)
                    {
                        Instantiate(powerUpParticle, enemiesInFight[i].transform.position, enemiesInFight[i].transform.rotation);
                        enemiesInFight[i].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (enemiesInFight[i].GetComponent<Unit>().CurrentHP >= enemiesInFight[i].GetComponent<Unit>().MaxHP)
                        {
                            enemiesInFight[i].GetComponent<Unit>().CurrentHP = enemiesInFight[i].GetComponent<Unit>().MaxHP;
                        }
                    }
                }

                //Pillar of Strength this increases the defense of the current caster by 50% for 3 turns
                else if (Combatants[currentCombatant].GetComponent<Unit>().pillarOfStrength)
                {
                    Instantiate(powerUpParticle, Combatants[currentCombatant].transform.position, Combatants[currentCombatant].transform.rotation);
                    Combatants[currentCombatant].GetComponent<Unit>().defenseMultiplier = .5f;
                    Combatants[currentCombatant].GetComponent<Unit>().defenseBonusTurnCount = 3;
                }

                //Potion of Healing this slightly heals one party member. There is an 80% chance they heal the weakest party member, 20% chance it randomly heals another person in the party
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionOfHealing)
                {
                    enemiesInFight = enemiesInFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();

                    int chanceOfPersonToHeal = Random.Range(0, 100);
                    if (chanceOfPersonToHeal <= 80)
                    {
                        enemiesInFight[0].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (enemiesInFight[0].GetComponent<Unit>().CurrentHP >= enemiesInFight[0].GetComponent<Unit>().MaxHP)
                        {
                            Instantiate(powerUpParticle, enemiesInFight[0].transform.position, enemiesInFight[0].transform.rotation);
                            enemiesInFight[0].GetComponent<Unit>().CurrentHP = enemiesInFight[0].GetComponent<Unit>().MaxHP;
                        }
                    }
                    else
                    {
                        int enemyToHeal = Random.Range(0, enemiesInFight.Count);
                        enemiesInFight[enemyToHeal].GetComponent<Unit>().CurrentHP += Combatants[currentCombatant].GetComponent<Unit>().healthToRecover;
                        if (enemiesInFight[enemyToHeal].GetComponent<Unit>().CurrentHP >= enemiesInFight[enemyToHeal].GetComponent<Unit>().MaxHP)
                        {
                            Instantiate(powerUpParticle, enemiesInFight[enemyToHeal].transform.position, enemiesInFight[enemyToHeal].transform.rotation);
                            enemiesInFight[enemyToHeal].GetComponent<Unit>().CurrentHP = enemiesInFight[enemyToHeal].GetComponent<Unit>().MaxHP;
                        }
                    }
                }

                //Potion of Resolve increases the attack of the current caster by 50% for 3 turns
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionofResolve)
                {
                    Instantiate(powerUpParticle, Combatants[currentCombatant].transform.position, Combatants[currentCombatant].transform.rotation);
                    Combatants[currentCombatant].GetComponent<Unit>().attackMultiplier = 1.5f;
                    Combatants[currentCombatant].GetComponent<Unit>().attackBonusTurnCount = 3;
                }

                //Potion of Resurrection: 1)Brings one party member back to life 2) Sets that party member's health to 50% 3) adds them back into the turn order 
                else if (Combatants[currentCombatant].GetComponent<Unit>().potionOfResurrection)
                {
                    for (int i = 0; i < enemiesInFight.Count; i++)
                    {
                        if (enemiesInFight[i].GetComponent<Unit>().characterIsDead)
                            deadCharacters.Add(enemiesInFight[i]);
                    }

                    if (deadCharacters.Count > 0)
                    {
                        Reincarnation();
                    }
                    else
                    {
                        print("No suitable target for resurrection, choose a different attack");
                        Combatants[currentCombatant].GetComponent<Unit>().RedoAttack();
                    }
                }
            }
        }
        #endregion

        UpdatePlayerHealthManaUI();

        //At the end of the turn, this cleans up/closes out any unnecessary value for all combatants
        for (int i = 0; i < Combatants.Count; i++)
        {
            Combatants[i].GetComponent<Unit>().CleanUp();
        }
    }

    public void Reincarnation()
    {
        //If reincarnated: 1) says they aren't dead 2} gives them 50% health 3)Adds them back to the combatants list 4) creates a particle effect 5) removes them from the deadCharacters list
        int deadPlayerChosen = Random.Range(0, deadCharacters.Count);
        deadCharacters[deadPlayerChosen].GetComponent<Unit>().characterIsDead = false;
        deadCharacters[deadPlayerChosen].GetComponent<Unit>().CurrentHP = (deadCharacters[deadPlayerChosen].GetComponent<Unit>().MaxHP * .5f);
        Combatants.Add(deadCharacters[deadPlayerChosen]);

        Instantiate(deadCharacters[deadPlayerChosen].GetComponent<Unit>().deathParticle, transform.position, transform.rotation);

        foreach (Transform child in Combatants[deadPlayerChosen].transform)
        {
            Instantiate(powerUpParticle, Combatants[deadPlayerChosen].transform.position, Combatants[deadPlayerChosen].transform.rotation);
            child.gameObject.SetActive(true);
        }
        deadCharacters[deadPlayerChosen].GetComponent<Unit>().anim.SetTrigger("Charge");
        deadCharacters.Remove(deadCharacters[deadPlayerChosen]);
    }

    public void CheckIfAllMembersKnockedDown()
    {
        //There is a system that *will be* integrated. If you hit someone with a 'critical' or 'weak', you will knock them down until their next turn. 
        //if you knock all the enemies (or players down) then you will launch an all-out attack
        //All Out Attack does damage to every enemy with a math formula similar to [(player1damage + player2damage + player3damage + player4damage) * .75f ]
        //all knocked down enemies are then no longer knocked down



        //If an enemy knocks down a player
        for (int i = 0; i < enemiesInFight.Count; i++)
        {
            if (enemiesInFight[i].GetComponent<Unit>().hasBeenKnockedDown)
            {
                playerCountForKnockedOut++;
            }
        }

        if (playerCountForKnockedOut == playersInThisFight.Count && !Combatants[currentCombatant].GetComponent<Unit>().isAPlayer && enemiesInFight.Count >= 2)
        {
            print("Enemy All Out Attack");

            //All players take damage based on who is still standing on the enemy side
            //Check if players are alive
            for (int i = 0; i < playersInThisFight.Count; i++)
            {
                playersInThisFight[i].GetComponent<Unit>().anim.SetBool("knockedDown", false);
            }
            //clear playerCountForKnockedOut
        }
        //If a player knocks down an enemy
        for (int i = 0; i < playersInThisFight.Count; i++)
        {
            if (playersInThisFight[i].GetComponent<Unit>().hasBeenKnockedDown)
            {
                enemyCountForKnockedOut++;
            }
        }

        if (enemyCountForKnockedOut == enemiesInFight.Count && Combatants[currentCombatant].GetComponent<Unit>().isAPlayer && playersInThisFight.Count >= 2)
        {
            print("Enemy All Out Attack");

            //All players take damage based on who is still standing on the enemy side
            //Check if players are alive
            //players stand back up
            //clear enemyCountForKnockedOut
        }
    }
    public void ExperienceAndDeathCollection()
    {
        //Experience and Death - this tracks: 1) who is left in the fight 2) has the player won or lost 

        //This first part - if either a player or enemy is killed, they are removed from the Combatants list (their turn is skipped in battle)
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

        //If an enemy is killed in battle, they are: 1) Removed from play 2) Removed from the list 3) their experience is added to a total number
        for (int i = 0; i < enemiesInFight.Count; i++)
        {
            if (enemiesInFight[i].GetComponent<Unit>().CurrentHP <= 0)
            {
                totalExperienceAwarded += Combatants[i].GetComponent<Unit>().experienceEarned;
                enemiesInFight.Remove(enemiesInFight[i]);
            }
        }

        //If there are no more enemies, the player has won
        if (enemiesInFight.Count <= 0)
        {
            state = StateOfBattle.WON;
            EndBattle();
        }

        //If the player is killed, there is a possibility they can be added back so we can't fully remove them. 
        //They are instead added to a dead players list (See Reincarnation() to come back)
        for (int i = 0; i < playersInThisFight.Count; i++)
        {
            if (playersInThisFight[i].GetComponent<Unit>().characterIsDead)
            {
                deadPlayers++;
            }
        }

        //Current structure is to not remove players from the PlayersInThisFight list as they can be called back
        //So right now, if the number of deadPlayers equals the number of players in the fight - everyone has been killed and the player loses
        if (deadPlayers >= playersInThisFight.Count)
        {
            state = StateOfBattle.LOST;
            EndBattle();
        }
    }

    public void UpdatePlayerHealthManaUI()
    {
        //This updates the player's UI in the bottom right hand corner (usually done at the end of the player's turn)
        for (int i = 0; i < playersInThisFight.Count; i++)
        {
            //If player's health < 0 their icons are turned black
            //This is hardcoded to be at 0 so the player doesn't see how negative their health is
            if (playersInThisFight[i].GetComponent<Unit>().CurrentHP <= 0)
            {
                healthText[i].text = "0";
                healthUI[i].value = 0;

                triangles[i].color = Color.black;
                icons[i].color = Color.black;
            }

            //otherwise, the stats are updated to reflected current state
            else
            {
                healthUI[i].value = playersInThisFight[i].GetComponent<Unit>().CurrentHP / playersInThisFight[i].GetComponent<Unit>().MaxHP;
                healthText[i].text = playersInThisFight[i].GetComponent<Unit>().CurrentHP.ToString("F0");

                triangles[i].color = GameManager.colorsInParty[i];
                icons[i].color = Color.white;
                icons[i].sprite = Resources.Load<Sprite>("PlayerIcons/" + playersInThisFight[i].GetComponent<Unit>().Name);
            }
            manaUI[i].value = playersInThisFight[i].GetComponent<Unit>().CurrentMP / playersInThisFight[i].GetComponent<Unit>().MaxMP;
            manaText[i].text = playersInThisFight[i].GetComponent<Unit>().CurrentMP.ToString("F0");
        }
    }
    public void EndTurn()
    {
        //this ends the turn and advances to the next one
        currentCombatant++;
        NextTurn();
    }

    void EndBattle()
    {
        ClearGameManager();
        //this just prints states, but will be used for triggering Experience windows (if won) or losing screen (if lost)
        if (state == StateOfBattle.LOST)
        {
            print("Player Lost");

        }
        else
        {
            Combatants[currentCombatant].GetComponent<Unit>().anim.SetTrigger("Victory");
            print("Player Won");
        }
    }

    //This clears any leftover data from GameManager picked up from 'RoamingMonster.cs' or weird data from the fight
    void ClearGameManager()
    {
        GameManager.enemyCount = 0;
        GameManager.enemiesInThisFight.Clear();
    }

}
