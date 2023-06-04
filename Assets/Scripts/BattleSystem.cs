using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { Start, Dani, Erebus, Phoebe, Tristan, Sophie, Theo, Party, EnemyTurn, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject DaniPrefab, ErebusPrefab, PhoebePrefab, TristanPrefab, SophiePrefab, TheoPrefab;
    Animator DaniAnim, ErebusAnim, PhoebeAnim, TristanAnim, SophieAnim, TheoAnim;

    //Initiated GameObjects
    GameObject playerGO1, playerGO2, playerGO3, playerGO4;

    public Transform DaniBattleStation;
    public Transform ErebusBattleStation;
    public Transform PhoebeBattleStation;
    public Transform TristanBattleStation;

    public Transform BattleStation1;
    public Transform BattleStation2;
    public Transform BattleStation3;
    public Transform BattleStation4;

    //The Player's Party
    BattleUnit Dani, Erebus, Phoebe, Tristan, Theo, Sophie;

    //UI for Health and Magic
    public Slider DaniHealth, DaniMagic;
    public Slider ErebusHealth, ErebusMagic;
    public Slider PhoebeHealth, PhoebeMagic;
    public Slider TristanHealth, TristanMagic;
    public TextMeshProUGUI DaniHText, DaniMText;
    public TextMeshProUGUI ErebusHText, ErebusMText;
    public TextMeshProUGUI PhoebeHText, PhoebeMText;
    public TextMeshProUGUI TristanHText, TristanMText;

    public TextMeshProUGUI friendshipBonusDisplay;

    bool enemySelect;
    bool playerSelect;
    bool isOver;

    //Particle System for selection
    public GameObject enemySelectionParticle;
    public GameObject playerSelectionParticle;

    //determining enemySelection
    public int enemyUnitSelected;
    public int playerUnitSelected;
    public List<Transform> playerBattleStationLocations;
    public List<Transform> enemyBattleStationLocations;
    public List<GameObject> enemyPrefab;
    public List<GameObject> enemyPrefabBoss;
    public List<BattleUnit> enemyUnit;
    public List<Animator> enemyAnim;

    bool DaniDead;
    bool ErebusDead;
    bool PhoebeDead;
    bool TristanDead;

    float damageToDo;
    bool isDead;

    bool successfulAttack;

    public TextMeshProUGUI dialogueText;

    public GameObject transParticle, charmsParticle, potionsParticle, physicalParticle, darkArtsParticle, ancientParticle, partyAttackParticle;

    public enum CharacterIdentifier
    { Player1, Player2, Player3, Player4, Party, Enemy1, Enemy2, Enemy3, Enemy4, Enemy5 };

    public List<CharacterIdentifier> playerTurnOrder = new List<CharacterIdentifier>();
    public List<CharacterIdentifier> EnemyTurnOrder = new List<CharacterIdentifier>();

    //BonusAttackCounter
    public int friendshipBonus = 0;

    public int playerCount = 4;
    int enemyStartCount = 0;
    int enemyCount = 0;

    int sequenceCount;
    bool playerStart;

    public bool isPlayerTurn;

    GameObject GameManagerObject;

    public Transform[] CameraStations;
    public GameObject cameraHolder;

    public GameObject DaniAttackMenus;

    bool attackAllEnemies;
    bool jumpToEndOfTurn;

    float attackPower;
    float currentAttackersMagic, currentAttackersPhys, currentWandAttunement, attackPercentOfUser = 1;
    float formulaicAttackBaseMagic, formulaicAttackBasePhys;

    //Choosing which player to attack
    int WhoToAttack;
    int currentEnemyAttacking;

    GameObject currentParty0, currentParty1, currentParty2, currentParty3;
    float currentParty0Agility, currentParty1Agility, currentParty2Agility, currentParty3Agility;

    Vector3 platformOffset = new Vector3(0, .5f, 0);
}
 /*   GameManager gameManager;
  /*  void Start()
    {
        currentParty0 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[0]), BattleStation1.transform.position + platformOffset, Quaternion.identity);
        currentParty1 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[1]), BattleStation2.transform.position + platformOffset, Quaternion.identity);
        currentParty2 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[2]), BattleStation3.transform.position + platformOffset, Quaternion.identity);
        currentParty3 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[3]), BattleStation4.transform.position + platformOffset, Quaternion.identity);

   /*     string temp = ("GameManager." + GameManager.inCurrentParty[0] + "Agility");
        print(temp);

        float.TryParse("GameManager.DaniAgility", out currentParty0Agility);
        print(currentParty0Agility);*/

        /*
        enemyStartCount = Random.Range(1, 5);
        playerStart = true;

        playerTurnOrder.Add(CharacterIdentifier.Player1);
        playerTurnOrder.Add(CharacterIdentifier.Player2);
        playerTurnOrder.Add(CharacterIdentifier.Player3);
        playerTurnOrder.Add(CharacterIdentifier.Player4);
        playerTurnOrder.Add(CharacterIdentifier.Party);

        EnemyTurnOrder.Add(CharacterIdentifier.Enemy1);

        if (enemyStartCount >= 2)
        {
            EnemyTurnOrder.Add(CharacterIdentifier.Enemy2);
        }
        if (enemyStartCount >= 3)
        {
            EnemyTurnOrder.Add(CharacterIdentifier.Enemy3);
        }
        if (enemyStartCount >= 4)
        {
            EnemyTurnOrder.Add(CharacterIdentifier.Enemy4);
        }
        if (enemyStartCount >= 5)
        {
            EnemyTurnOrder.Add(CharacterIdentifier.Enemy5);
        }

        state = BattleState.START;

        if (GameManager.DaniCurrentHP <= 0)
        {
            GameManager.DaniCurrentHP = 1;
        }
        if (GameManager.ErebusCurrentHP <= 0)
        {
            GameManager.ErebusCurrentHP = 1;
        }
        if (GameManager.PhoebeCurrentHP <= 0)
        {
            GameManager.PhoebeCurrentHP = 1;
        }
        if (GameManager.TristanCurrentHP <= 0)
        {
            GameManager.TristanCurrentHP = 1;
        }

        for (int i = 0; i < enemyStartCount; i++)
        {
            enemyCount++;
        }

        DaniDead = false;
        ErebusDead = false;
        PhoebeDead = false;
        TristanDead = false;

        UpdateLifeUI();
        UpdateMagicUI();

        GameManagerObject = GameObject.Find("GameManager");

        StartCoroutine(SetUpBattle());*/
  /*  }
    IEnumerator SetUpBattle()
    {
        //This places all the UI and assigns the player and enemy values from the Unit Script

        playerGO2 = Instantiate(DaniPrefab, DaniBattleStation);
        Dani = playerGO2.GetComponent<BattleUnit>();
        DaniAnim = playerGO2.GetComponentInChildren<Animator>();

        playerGO3 = Instantiate(ErebusPrefab, ErebusBattleStation);
        Erebus = playerGO3.GetComponent<BattleUnit>();
        ErebusAnim = playerGO3.GetComponentInChildren<Animator>();

        playerGO1 = Instantiate(PhoebePrefab, PhoebeBattleStation);
        Phoebe = playerGO1.GetComponent<BattleUnit>();
        PhoebeAnim = playerGO1.GetComponentInChildren<Animator>();

        playerGO4 = Instantiate(TristanPrefab, TristanBattleStation);
        Tristan = playerGO4.GetComponent<BattleUnit>();
        TristanAnim = playerGO4.GetComponentInChildren<Animator>();

        for (int i = 0; i < enemyStartCount; i++)
        {
            int RandEnemy = Random.Range(0, enemyPrefab.Count);
            GameObject enemyGO = Instantiate(enemyPrefab[RandEnemy], enemyBattleStationLocations[i]);
            enemyUnit.Add(enemyGO.GetComponent<BattleUnit>());
            if(enemyUnit[i].unitName == "Soucouyant")
                enemyAnim.Add(GameObject.Find("Demon").GetComponent<Animator>());
            else
                enemyAnim.Add(enemyGO.GetComponentInChildren<Animator>());
            enemyUnit[i].transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        enemyUnit[0].GetComponent<BattleUnit>().myEnumValue = CharacterIdentifier.Enemy1;
        if (enemyStartCount >= 2)
        {
            enemyUnit[1].GetComponent<BattleUnit>().myEnumValue = CharacterIdentifier.Enemy2;
        }
        if (enemyStartCount >= 3)
        {
            enemyUnit[2].GetComponent<BattleUnit>().myEnumValue = CharacterIdentifier.Enemy3;
        }
        if (enemyStartCount >= 4)
        {
            enemyUnit[3].GetComponent<BattleUnit>().myEnumValue = CharacterIdentifier.Enemy4;
        }
        if (enemyStartCount >= 5)
        {
            enemyUnit[4].GetComponent<BattleUnit>().myEnumValue = CharacterIdentifier.Enemy5;
        }

        yield return new WaitForSeconds(2f);

        NextTurn();
    }

    private void Update()
    {
        //Cheat Code to Win Battle
        /*  {
              if (Input.GetKeyDown(KeyCode.E))
              {
                  CheatToInstantlyWin();
              }
          }
        */
        //If MC Health <= 0, game is over (at the bottom of this function)
  /*      if (GameManager.DaniCurrentHP <= 0)
        {
            playerTurnOrder.Remove(CharacterIdentifier.Player1);
        }
        if (GameManager.ErebusCurrentHP <= 0)
        {
            playerTurnOrder.Remove(CharacterIdentifier.Player2);
        }

        if (GameManager.PhoebeCurrentHP <= 0)
        {
            playerTurnOrder.Remove(CharacterIdentifier.Player3);
        }

        if (GameManager.TristanCurrentHP <= 0)
        {
            playerTurnOrder.Remove(CharacterIdentifier.Player4);
        }

        if (GameManager.DaniCurrentHP <= 0 && GameManager.ErebusCurrentHP <= 0 && GameManager.PhoebeCurrentHP <= 0 && GameManager.TristanCurrentHP <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
        }

        //Adding back into party after knockout
        if (GameManager.DaniCurrentHP >= 0 && DaniDead)
        {
            DaniAnim.SetBool("isDead", false);
            DaniDead = false;
            playerTurnOrder.Add(CharacterIdentifier.Player1);
        }
        if (GameManager.ErebusCurrentHP >= 0 && ErebusDead)
        {
            ErebusAnim.SetBool("isDead", false);
            ErebusDead = false;
            playerTurnOrder.Add(CharacterIdentifier.Player2);
        }
        if (GameManager.PhoebeCurrentHP >= 0 && PhoebeDead)
        {
            PhoebeAnim.SetBool("isDead", false);
            PhoebeDead = false;
            playerTurnOrder.Add(CharacterIdentifier.Player3);
        }
        if (GameManager.TristanCurrentHP >= 0 && TristanDead)
        {
            TristanAnim.SetBool("isDead", false);
            TristanDead = false;
            playerTurnOrder.Add(CharacterIdentifier.Player4);
        }

        #region Select Enemy
        if ((state == BattleState.Dani /*|| state == BattleState.Erebus || state == BattleState.Phoebe || state == BattleState.TRISTAN*//*) && enemySelect)
  /*      {
            UpdateEnemyTargetedUI(enemyUnitSelected);


            //SelectionProcess
            enemySelectionParticle.SetActive(true);
            enemySelectionParticle.transform.position = enemyBattleStationLocations[enemyUnitSelected].transform.position;

            for (int i = 0; i < enemyBattleStationLocations[enemyUnitSelected].transform.childCount; i++)
            {
                enemyBattleStationLocations[enemyUnitSelected].transform.GetChild(i).gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.A) && enemyStartCount > 1)
            {
                enemyUnitSelected--;
                if (enemyUnitSelected < 0)
                {
                    enemyUnitSelected = enemyBattleStationLocations.Count - (6 - enemyStartCount);
                    UpdatePreviousEnemyTargetedUI(0);
                    UpdateEnemyTargetedUI(enemyBattleStationLocations.Count - (6 - enemyStartCount));
                }
                else
                {
                    UpdatePreviousEnemyTargetedUI(enemyUnitSelected + 1);
                    UpdateEnemyTargetedUI(enemyUnitSelected);
                }
                enemySelectionParticle.transform.position = enemyBattleStationLocations[enemyUnitSelected].transform.position;
                cameraHolder.transform.LookAt(enemyBattleStationLocations[enemyUnitSelected]);
            }
            if (Input.GetKeyDown(KeyCode.D) && enemyStartCount > 1)
            {
                enemyUnitSelected++;
                if (enemyUnitSelected >= enemyStartCount)
                {
                    enemyUnitSelected = 0;
                    UpdatePreviousEnemyTargetedUI(enemyBattleStationLocations.Count - (6 - enemyStartCount));
                    UpdateEnemyTargetedUI(0);
                }
                else
                {
                    UpdatePreviousEnemyTargetedUI(enemyUnitSelected - 1);
                    UpdateEnemyTargetedUI(enemyUnitSelected);
                }

                enemySelectionParticle.transform.position = enemyBattleStationLocations[enemyUnitSelected].transform.position;
                cameraHolder.transform.LookAt(enemyBattleStationLocations[enemyUnitSelected]);
            }

            if (state == BattleState.Dani)
            {
                Dani.transform.LookAt(enemyBattleStationLocations[enemyUnitSelected]);
            }

            if (state == BattleState.Erebus)
            {
                Erebus.transform.LookAt(enemyBattleStationLocations[enemyUnitSelected]);
            }

            if (state == BattleState.Phoebe)
            {
                Phoebe.transform.LookAt(enemyBattleStationLocations[enemyUnitSelected]);
            }

            if (state == BattleState.Tristan)
            {
                Tristan.transform.LookAt(enemyBattleStationLocations[enemyUnitSelected]);
            }
        }
        else
        {
            enemySelectionParticle.SetActive(false);
        }

        if (enemyCount == 0 && !isOver)
        {
            state = BattleState.WON;
            EndBattle();
        }
        #endregion

        if (friendshipBonus == 0)
            friendshipBonusDisplay.text = "";
        else
            friendshipBonusDisplay.text = friendshipBonus.ToString();

        #region Select Team Member
        if ((state == BattleState.Dani || state == BattleState.Erebus || state == BattleState.Phoebe || state == BattleState.Tristan) && playerSelect)
        {
            //SelectionProcess
            playerSelectionParticle.SetActive(true);

            int membersInParty = playerTurnOrder.Count;

            playerSelectionParticle.transform.position = playerBattleStationLocations[playerUnitSelected].transform.position;

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (playerUnitSelected <= 0)
                {
                    playerUnitSelected = membersInParty;
                }
                print(playerUnitSelected);
                playerUnitSelected--;
                playerSelectionParticle.transform.position = playerBattleStationLocations[playerUnitSelected].transform.position;
                // Camera.transform.LookAt(playerBattleStationLocations[playerUnitSelected]);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                playerUnitSelected++;
                if (playerUnitSelected >= membersInParty)
                {
                    playerUnitSelected = 0;
                }
                print(playerUnitSelected);
                playerSelectionParticle.transform.position = playerBattleStationLocations[playerUnitSelected].transform.position;
                // Camera.transform.LookAt(playerBattleStationLocations[playerUnitSelected]);
            }
        }
        #endregion
    }

    void UpdateEnemyTargetedUI(int target)
    {
        GameObject enemyUnitInfo = enemyUnit[target].transform.GetChild(1).gameObject;
        GameObject enemyUnitHealth = enemyUnit[target].transform.GetChild(2).gameObject;

        enemyUnit[enemyUnitSelected].CheckIfAffinityKnown();

        enemyUnitInfo.SetActive(true);
        enemyUnitHealth.SetActive(true);
    }

    void UpdatePreviousEnemyTargetedUI(int target)
    {
        GameObject enemyUnitInfo = enemyUnit[target].transform.GetChild(1).gameObject;
        GameObject enemyUnitHealth = enemyUnit[target].transform.GetChild(2).gameObject;

        enemyUnitInfo.SetActive(false);
        enemyUnitHealth.SetActive(false);
    }
    void NextTurn()
    {
        print("Im on my next turn");
       /* isPlayerTurn = !isPlayerTurn;
        jumpToEndOfTurn = false;

        CharacterIdentifier upRightNow;
        if (isPlayerTurn)
        {
            upRightNow = playerTurnOrder[0];
            playerTurnOrder.RemoveAt(0);
            playerTurnOrder.Add(upRightNow);
        }

        else
        {
            if (enemyCount > 0)
            {
                float upNextHealth = 0;

                do
                {
                    upRightNow = EnemyTurnOrder[0];
                    EnemyTurnOrder.RemoveAt(0);
                    EnemyTurnOrder.Add(upRightNow);

                    switch (upRightNow)
                    {
                        case CharacterIdentifier.Enemy1:
                            upNextHealth = enemyUnit[0].unitMaxHP;
                            break;
                        case CharacterIdentifier.Enemy2:
                            upNextHealth = enemyUnit[1].unitMaxHP;
                            break;
                        case CharacterIdentifier.Enemy3:
                            upNextHealth = enemyUnit[2].unitMaxHP;
                            break;
                        case CharacterIdentifier.Enemy4:
                            upNextHealth = enemyUnit[3].unitMaxHP;
                            break;
                        case CharacterIdentifier.Enemy5:
                            upNextHealth = enemyUnit[4].unitMaxHP;
                            break;

                        default:
                            Debug.LogError("This should only be handeling enemy turns");
                            upNextHealth = 0;
                            break;
                    }

                } while (upNextHealth <= 0);
            }
            else
            {
                upRightNow = 0;
                state = BattleState.WON;
                EndBattle();
            }
        }

        switch (upRightNow)
        {
            case CharacterIdentifier.Player1:
                DaniTurn();
                state = BattleState.Dani;
                break;
            case CharacterIdentifier.Player2:
                ErebusTurn();
                state = BattleState.Erebus;
                break;
            case CharacterIdentifier.Player3:
                PhoebeTurn();
                state = BattleState.Phoebe;
                break;
            case CharacterIdentifier.Player4:
                TristanTurn();
                state = BattleState.Tristan;
                break;
            case CharacterIdentifier.Party:
                FriendshipAttack();
                state = BattleState.Party;
                break;
            case CharacterIdentifier.Enemy1:
                state = BattleState.EnemyTurn;
                StartCoroutine("EnemyTurn", 0);
                break;
            case CharacterIdentifier.Enemy2:
                state = BattleState.EnemyTurn;
                StartCoroutine("EnemyTurn", 1);
                break;
            case CharacterIdentifier.Enemy3:
                state = BattleState.EnemyTurn;
                StartCoroutine("EnemyTurn", 2);
                break;
            case CharacterIdentifier.Enemy4:
                state = BattleState.EnemyTurn;
                StartCoroutine("EnemyTurn", 3);
                break;
        }*/
 /*   }

    bool isPlayerIndexDead(int playerID)
    {
        switch (playerID)
        {
            case 0:
                return DaniDead;
            case 1:
                return ErebusDead;
            case 2:
                return PhoebeDead;
            case 3:
                return TristanDead;
        }
        return false;
    }
    private string ReturnNameOfEnemy(CharacterIdentifier forID)
    {
        for (int i = 0; i < enemyUnit.Count; i++)
        {
            if (enemyUnit[i].myEnumValue == forID)
            {
                string nameToRemoveCloneFrom = enemyUnit[i].name;
                nameToRemoveCloneFrom = nameToRemoveCloneFrom.Replace("(Clone)", "");
                return nameToRemoveCloneFrom;
            }
        }
        return "Error: No Match Found";
    }

    void DaniTurn()
    {
        if (Dani.DefenseCounter > 0)
            Dani.DefenseCounter -= 1;
        if (Dani.DefenseCounter <= 0)
            Dani.defensePercent = 0;

        cameraHolder.transform.parent = CameraStations[0].transform;
        enemySelect = true;
        cameraHolder.transform.localPosition = new Vector3(0, 0, 0);
        print("Dani Turn");
        DaniAttackMenus.SetActive(true);
    }

    void ErebusTurn()
    {
        if (Erebus.DefenseCounter > 0)
            Erebus.DefenseCounter -= 1;
        if (Erebus.DefenseCounter <= 0)
            Erebus.defensePercent = 0;

        cameraHolder.transform.parent = CameraStations[1].transform;
        enemySelect = true;
        cameraHolder.transform.localPosition = new Vector3(0, 0, 0);
        print("Erebus Turn");
        DaniAttackMenus.SetActive(true);
    }

    void PhoebeTurn()
    {
        if (Phoebe.DefenseCounter > 0)
            Phoebe.DefenseCounter -= 1;
        if (Phoebe.DefenseCounter <= 0)
            Phoebe.defensePercent = 0;

        cameraHolder.transform.parent = CameraStations[2].transform;
        enemySelect = true;
        cameraHolder.transform.localPosition = new Vector3(0, 0, 0);
        print("Phoebe Turn");
        DaniAttackMenus.SetActive(true);
    }

    void TristanTurn()
    {
        if (Tristan.DefenseCounter > 0)
            Tristan.DefenseCounter -= 1;
        if (Tristan.DefenseCounter <= 0)
            Tristan.defensePercent = 0;

        cameraHolder.transform.parent = CameraStations[3].transform;
        enemySelect = true;
        cameraHolder.transform.localPosition = new Vector3(0, 0, 0);
        print("Tristan Turn");
        DaniAttackMenus.SetActive(true);
    }

    void FriendshipAttack()
    {
        if (friendshipBonus > 0)
        {
            print("LAUNCH ATTACK");

            for (int i = 0; i < enemyUnit.Count; i++)
            {
                enemyUnit[i].Ultimate = true;
            }
            attackAllEnemies = true;

            damageToDo = ((10+10+10+10)/4) * friendshipBonus;
            //eventually modify this to -> ([party's attack 1,2,3,4 / 4] * friendshipbonus

            DaniPostAttackItems(0);
        }
        else
        {
            StartCoroutine(WaitForEndOfTurn());
        }
          friendshipBonus = 0;
    }
    IEnumerator EnemyTurn(int enemyIndex)
    {
        currentEnemyAttacking = enemyIndex;
        UpdatePreviousEnemyTargetedUI(enemyUnitSelected);

        cameraHolder.transform.parent = CameraStations[4].transform;
        cameraHolder.transform.localPosition = new Vector3(0, 0, 0);
        print("Enemy Turn");

        if (enemyUnit[enemyIndex].isPoisoned && !enemyUnit[enemyIndex].StrPo)
        {
            isDead = enemyUnit[enemyIndex].TakeDamage(enemyUnit[enemyIndex].poisonedDamage);

            //update UI 
            enemyUnit[enemyIndex].poisonedTurnCount -= 1;
            if (enemyUnit[enemyIndex].poisonedTurnCount <= 0)
            {
                enemyUnit[enemyIndex].isPoisoned = false;
                enemyUnit[enemyIndex].poisonedParticle.SetActive(false);
            }

            if (isDead)
            {
                print("Enemy is now dead");
                RemoveCurrentEnemy();
                jumpToEndOfTurn = true;
                StartCoroutine(WaitForEndOfTurn());
            }
        }


        //Checking at the end of the turn if the Enemy Unit is not paralyzed anymore
        if (enemyUnit[enemyIndex].isParalyzed)
        {
            int chanceToHit = Random.Range(0, 100);
            if (enemyUnit[enemyUnitSelected].agility > chanceToHit)
            {
                enemyUnit[enemyIndex].isParalyzed = false;
                enemyUnit[enemyIndex].paralyzedCounter = 0;
                enemyUnit[enemyIndex].anim.enabled = true;
            }
            else
            {
                enemyUnit[enemyIndex].paralyzedCounter -= 1;
                if (enemyUnit[enemyIndex].paralyzedCounter <= 0)
                {
                    enemyUnit[enemyIndex].isParalyzed = false;
                    enemyUnit[enemyIndex].anim.enabled = true;
                }
            }
        }

        /* else if (enemyUnit[enemyIndex].isDizzy)
         {
             int chanceToHit = Random.Range(0, 100);
             if (enemyUnit[enemyUnitSelected].agility > chanceToHit)
             {
                 enemyUnit[enemyIndex].isDizzy = false;
                 enemyUnit[enemyIndex].dizzyTurnCount = 0;
                 enemyUnit[enemyIndex].dizzyParticle.SetActive(false);
             }
             else
             {
                 //Attack something else
  //    //      //     //COME BACK TO THIS
                 enemyUnit[enemyIndex].dizzyTurnCount -= 1;
                 if (enemyUnit[enemyIndex].dizzyTurnCount <= 0)
                 {
                     enemyUnit[enemyIndex].isDizzy = false;
                     enemyUnit[enemyIndex].dizzyParticle.SetActive(false);
                 }
             }
         }*/

   /*     else
        {
            int RandomAttack = Random.Range(0, 100);

            //Choosing Who To Attack happens at least once, if it is true, it does it again. (keep going until valid)
            int safteyCounter = 1000;
            bool viableTarget = false;

            do
            {
                WhoToAttack = Random.Range(0, 4);
                if (safteyCounter-- < 0)
                {
                    Debug.LogError("Couldn't find a living WhoToAttack, is the Whole Team Dead?");
                    break;
                    //bails us out of the do while
                }
                if (isPlayerIndexDead(WhoToAttack) == false)
                {
                    if (WhoToAttack == 0 && !DaniDead)
                    {
                        Debug.Log("attacking player");
                        viableTarget = true;
                    }

                    else if (WhoToAttack == 1 && !ErebusDead)
                    {
                        viableTarget = true;
                    }
                    else if (WhoToAttack == 2 && !PhoebeDead)
                    {
                        viableTarget = true;
                    }
                    else if (WhoToAttack == 3 && !TristanDead)
                    {
                        viableTarget = true;
                    }
                }//Player not dead
            } while (viableTarget == false);

            yield return new WaitForSeconds(1.5f);

            Debug.Log("attack player with damage");
            enemyUnit[enemyIndex].transform.LookAt(playerBattleStationLocations[WhoToAttack].transform.position);

            cameraHolder.transform.LookAt(playerBattleStationLocations[WhoToAttack].transform.position);
            //Choose Attack
            int randomAttack = Random.Range(0, enemyUnit[enemyIndex].attacksKnown.Count);
            print(enemyUnit[enemyIndex].attacksKnown[randomAttack]);

            Invoke(enemyUnit[enemyIndex].attacksKnown[randomAttack], 0f);
            yield return new WaitForSeconds(.5f);

            //COME BACK TO
            //if they are dizzy, attack one of their own (or themselves)

            yield return new WaitForSeconds(2);
        }   
        if (jumpToEndOfTurn)
        {
            StartCoroutine(WaitForEndOfTurn());
        }
    }

    void UpdateLifeUI()
    {
        DaniHealth.value = GameManager.DaniCurrentHP / GameManager.DaniMaxHP;
        ErebusHealth.value = GameManager.ErebusCurrentHP / GameManager.ErebusMaxHP;
        PhoebeHealth.value = GameManager.PhoebeCurrentHP / GameManager.PhoebeMaxHP;
        TristanHealth.value = GameManager.TristanCurrentHP / GameManager.TristanMaxHP;

        DaniHText.text = GameManager.DaniCurrentHP.ToString("F0") + "/" + GameManager.DaniMaxHP.ToString("F0"); 
        ErebusHText.text = GameManager.ErebusCurrentHP.ToString("F0") + "/" + GameManager.ErebusMaxHP.ToString("F0"); 
        PhoebeHText.text = GameManager.PhoebeCurrentHP.ToString("F0") + "/" + GameManager.PhoebeMaxHP.ToString("F0"); 
        TristanHText.text = GameManager.TristanCurrentHP.ToString("F0") + "/" + GameManager.TristanMaxHP.ToString("F0"); 
    }

    void UpdateMagicUI()
    {
        DaniMagic.value = GameManager.DaniCurrentMP / GameManager.DaniMaxMP;
        ErebusMagic.value = GameManager.ErebusCurrentMP / GameManager.ErebusMaxMP;
        PhoebeMagic.value = GameManager.PhoebeCurrentMP / GameManager.PhoebeMaxMP;
        TristanMagic.value = GameManager.TristanCurrentMP / GameManager.TristanMaxMP;

        DaniMText.text = GameManager.DaniCurrentMP + "/" + GameManager.DaniMaxMP;
        ErebusMText.text = GameManager.ErebusCurrentMP + "/" + GameManager.ErebusMaxMP;
        PhoebeMText.text = GameManager.PhoebeCurrentMP + "/" + GameManager.PhoebeMaxMP;
        TristanMText.text = GameManager.TristanCurrentMP + "/" + GameManager.TristanMaxMP;
    }

    void EndBattle()
    {
        if (state == BattleState.LOST)
        {
            print("Player Lost");
        }
        else
        {
            print("Player Won");
        }
    }

    #region InitalAttacks - Not Used
    void DaniPostAttackItems(int MPCost)
    {
        GameManager.DaniCurrentMP -= MPCost;
        DaniMagic.value = GameManager.DaniCurrentMP;
        DaniAnim.SetTrigger("Attack");

        DaniAttackMenus.SetActive(false);
        enemySelectionParticle.SetActive(false);

        UpdateMagicUI();

        int chanceToHit = Random.Range(0, 100);
        if (enemyUnit[enemyUnitSelected].agility > chanceToHit)
        {
            damageToDo = 0;
            //Fix this later - add text that says 'Missed' 
            print("target missed");
            ClearEnemyAttributes();
            StartCoroutine(WaitForEndOfTurn());
        }
        else
        {
            if (enemyUnit[enemyUnitSelected].unitCurrentHP <= 0)
            {
                dialogueText.text = "Enemy is knocked out, select another target.";
                dialogueText.text = "Select someone to attack!";

                ClearEnemyAttributes();
                //DaniMenu.SetActive(true);
                //DaniSpells.SetActive(false);
            }
            else
            {
                float chanceForCritical = Random.Range(0, 100);
                if (GameManager.DaniFinesse >= chanceForCritical)
                {
                    enemyUnit[enemyUnitSelected].criticalLanded = true;
                }
                successfulAttack = true;
                CreateFullParticle();
            }
        }
    }
    public void DaniAttack()
    {
        // We turn on the bool for the enemy
        enemyUnit[enemyUnitSelected].DA = true;

        //isDead = enemyUnit[enemyUnitSelected].ThrowRock(MC.MCSpell1Damage * AttackModifier + GameManager.MCDodge);

        //This is defaulting to a negative number, so if you want it to go down, put in a positive one
        DaniPostAttackItems(5);
    }

    void DaniAgilityChange()
    { 
    //This is not currently slated for the demo
    }

    public void DaniParalyzeAttack()
    {
        DaniPostAttackItems(0);

        if (successfulAttack)
        {
            print("Enemy has been paralyzed");
            enemyUnit[enemyUnitSelected].anim.enabled = false;
            enemyUnit[enemyUnitSelected].isParalyzed = true;
            enemyUnit[enemyUnitSelected].paralyzedCounter = 3;
        }
    }

    public void DaniDecreaseDefense()
    {
        DaniPostAttackItems(0);

        if (successfulAttack)
        {
            print("Enemy has Defense Lowered");

            //Positive is decrease, negative is increase (1 - .5f) = .5 = less damage
            enemyUnit[enemyUnitSelected].defensePercent = .5f;
            enemyUnit[enemyUnitSelected].DefenseCounter = 3;
        }
    }

    public void DaniDecreaseAttack()
    {
        DaniPostAttackItems(0);

        if (successfulAttack)
        {
            print("Enemy has Attack Lowered");

            enemyUnit[enemyUnitSelected].attackPercent = -.5f;
            enemyUnit[enemyUnitSelected].AttackCounter = 3;
        }
    }

    public void DaniAttackAllEnemies()
    {
        // We turn on the bool for the enemy
        for (int i = 0; i < enemyUnit.Count; i++)
        {
            enemyUnit[i].Trans = true;
        }
        attackAllEnemies = true; 

        damageToDo = 15;
        //isDead = enemyUnit[enemyUnitSelected].ThrowRock(MC.MCSpell1Damage * AttackModifier + GameManager.MCDodge);

        //This is defaulting to a negative number, so if you want it to go down, put in a positive one
        DaniPostAttackItems(10);
    }

    public void DaniPoison()
    {
        enemyUnit[enemyUnitSelected].Potions = true;

        DaniPostAttackItems(5);

        if (successfulAttack)
        {
            if (!enemyUnit[enemyUnitSelected].StrPo)
            {
                enemyUnit[enemyUnitSelected].isPoisoned = true;
            }
            damageToDo = 3;
        }
    }

    public void DaniDizzy()
    {
        enemyUnit[enemyUnitSelected].Ancient = true;

        DaniPostAttackItems(5);

        if (successfulAttack)
        {
            enemyUnit[enemyUnitSelected].isDizzy = true;
            enemyUnit[enemyUnitSelected].dizzyTurnCount = 3;
        }
    }
    #endregion

    #region Full List of Attacks in Game

    void PostAttackItems(int MPCost)
    {
        if (state == BattleState.Dani)
        {
            GameManager.DaniCurrentMP -= MPCost;
            DaniMagic.value = GameManager.DaniCurrentMP;
            DaniAnim.SetTrigger("Attack");

            DaniAttackMenus.SetActive(false);
            enemySelectionParticle.SetActive(false);
        }
        else if (state == BattleState.Erebus)
        {
            GameManager.ErebusCurrentMP -= MPCost;
            ErebusMagic.value = GameManager.ErebusCurrentMP;
            ErebusAnim.SetTrigger("Attack");

          //  ErebusAttackMenus.SetActive(false);
            enemySelectionParticle.SetActive(false);
        }
        else if (state == BattleState.Phoebe)
        {
            GameManager.PhoebeCurrentMP -= MPCost;
            PhoebeMagic.value = GameManager.PhoebeCurrentMP;
            PhoebeAnim.SetTrigger("Attack");

            //  ErebusAttackMenus.SetActive(false);
            enemySelectionParticle.SetActive(false);
        }
        else if (state == BattleState.Tristan)
        {
            GameManager.TristanCurrentMP -= MPCost;
            TristanMagic.value = GameManager.TristanCurrentMP;
            TristanAnim.SetTrigger("Attack");

            //  ErebusAttackMenus.SetActive(false);
            enemySelectionParticle.SetActive(false);
        }

        else if (state == BattleState.EnemyTurn)
        {
            enemyAnim[currentEnemyAttacking].SetTrigger("Attack");
        }

        UpdateMagicUI();

        int chanceToHit = Random.Range(0, 100);
        if (state != BattleState.EnemyTurn)
        {
            if (enemyUnit[enemyUnitSelected].agility > chanceToHit)
            {
                damageToDo = 0;
                //Fix this later - add text that says 'Missed' 
                print("target missed");
                ClearEnemyAttributes();
                StartCoroutine(WaitForEndOfTurn());
            }
            else
            {
                if (enemyUnit[enemyUnitSelected].unitCurrentHP <= 0)
                {
                    dialogueText.text = "Enemy is knocked out, select another target.";
                    dialogueText.text = "Select someone to attack!";

                    ClearEnemyAttributes();
                    //DaniMenu.SetActive(true);
                    //DaniSpells.SetActive(false);
                }
                else
                {
                    float chanceForCritical = Random.Range(0, 100);
                    if (state == BattleState.Dani && GameManager.DaniFinesse >= chanceForCritical)
                    {
                        enemyUnit[enemyUnitSelected].criticalLanded = true;
                    }
                    else if (state == BattleState.Erebus && GameManager.ErebusFinesse >= chanceForCritical)
                    {
                        enemyUnit[enemyUnitSelected].criticalLanded = true;
                    }
                    else if (state == BattleState.Phoebe && GameManager.PhoebeFinesse >= chanceForCritical)
                    {
                        enemyUnit[enemyUnitSelected].criticalLanded = true;
                    }
                    else if (state == BattleState.Tristan && GameManager.TristanFinesse >= chanceForCritical)
                    {
                        enemyUnit[enemyUnitSelected].criticalLanded = true;
                    }
                    successfulAttack = true;
                   // CreateFullParticle();
                }
            }
        }
        else  //this is the enemy attacking
        {
            if (attackAllEnemies)
            {
                if (Dani.agility > chanceToHit)
                {
                    damageToDo = 0;
                    //Fix this later - add text that says 'Missed' 
                    print("target missed");
                }
                if (Erebus.agility > chanceToHit)
                {
                    damageToDo = 0;
                    //Fix this later - add text that says 'Missed' 
                    print("target missed");
                }
                if (Phoebe.agility > chanceToHit)
                {
                    damageToDo = 0;
                    //Fix this later - add text that says 'Missed' 
                    print("target missed");
                }
                if (Tristan.agility > chanceToHit)
                {
                    damageToDo = 0;
                    //Fix this later - add text that says 'Missed' 
                    print("target missed");
                }
                else
                {
                    //We'll start with a flat 20% chance for enemy critical
                    float chanceForCritical = Random.Range(0, 100);
                    if (20 >= chanceForCritical)
                    {
                        Dani.criticalLanded = true;
                        Erebus.criticalLanded = true;
                        Phoebe.criticalLanded = true;
                        Tristan.criticalLanded = true;
                    }
                    successfulAttack = true;
                    CreateFullParticle();
                }
                ClearEnemyAttributes();
                StartCoroutine(WaitForEndOfTurn());
            }
            else if (WhoToAttack == 0 && Dani.agility > chanceToHit)
            {
                damageToDo = 0;
                //Fix this later - add text that says 'Missed' 
                print("target missed");
                ClearEnemyAttributes();
                StartCoroutine(WaitForEndOfTurn());
            }
            else if (WhoToAttack == 1 && Erebus.agility > chanceToHit)
            {
                damageToDo = 0;
                //Fix this later - add text that says 'Missed' 
                print("target missed");
                ClearEnemyAttributes();
                StartCoroutine(WaitForEndOfTurn());
            }
            else if (WhoToAttack == 2 && Phoebe.agility > chanceToHit)
            {
                damageToDo = 0;
                //Fix this later - add text that says 'Missed' 
                print("target missed");
                ClearEnemyAttributes();
                StartCoroutine(WaitForEndOfTurn());
            }
            else if (WhoToAttack == 3 && Tristan.agility > chanceToHit)
            {
                damageToDo = 0;
                //Fix this later - add text that says 'Missed' 
                print("target missed");
                ClearEnemyAttributes();
                StartCoroutine(WaitForEndOfTurn());
            }

            else
            {
                //We'll start with a flat 20% chance for enemy critical
                float chanceForCritical = Random.Range(0, 100);
                if (20 >= chanceForCritical)
                {
                    Dani.criticalLanded = true;
                    Erebus.criticalLanded = true;
                    Phoebe.criticalLanded = true;
                    Tristan.criticalLanded = true;
                }
                successfulAttack = true;
                CreateFullParticle();
            }
        }
    }


    #region Ancient
    public void Ajei()
    {
        GetAttackersMagAndPhys();
        if(state != BattleState.EnemyTurn)
            enemyUnit[enemyUnitSelected].Ancient = true;
        else {
            if (WhoToAttack == 0)
                Dani.Ancient = true;
            else if (WhoToAttack == 1)
                Erebus.Ancient = true;
            else if (WhoToAttack == 2)
                Phoebe.Ancient = true;
            else if (WhoToAttack == 3)
                Tristan.Ancient = true;
        }

        attackPower = .6f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }

    public void Deeza()
    {
        GetAttackersMagAndPhys();
        if (state != BattleState.EnemyTurn)
            enemyUnit[enemyUnitSelected].Ancient = true;
        else
        {
            if (WhoToAttack == 0)
                Dani.Ancient = true;
            else if (WhoToAttack == 1)
                Erebus.Ancient = true;
            else if (WhoToAttack == 2)
                Phoebe.Ancient = true;
            else if (WhoToAttack == 3)
                Tristan.Ancient = true;
        }

        attackPower = 1.1f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }

    public void Hookeeghan()
    {
        GetAttackersMagAndPhys();

        if (state != BattleState.EnemyTurn)
        {
            for (int i = 0; i < enemyUnit.Count; i++)
            {
                enemyUnit[i].Ancient = true;
            }
            enemyUnit[enemyUnitSelected].Ancient = true;
        }
        else
        {
            Dani.Ancient = true;
            Erebus.Ancient = true;
            Phoebe.Ancient = true;
            Tristan.Ancient = true;
        }

        attackAllEnemies = true;

        attackPower = 1.1f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }
    #endregion

    #region Charms
    public void AuraDEau()
    {
        GetAttackersMagAndPhys();
        enemyUnit[enemyUnitSelected].Charms = true;

        attackPower = .6f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);

        if (successfulAttack)
        {
            //high percent chance to paralyze
            int percentToPoison = Random.Range(0, 100);
            if (percentToPoison < 75)
            {
                if (!enemyUnit[enemyUnitSelected].StrPo)
                {
                    enemyUnit[enemyUnitSelected].isPoisoned = true;
                    print("Enemy has been poisoned");
                }
                damageToDo = 3;
            }
        }
    }

    public void CriDeGivre()
    {
        GetAttackersMagAndPhys();
        enemyUnit[enemyUnitSelected].Charms = true;

        attackPower = 1.1f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);

        if (successfulAttack)
        {
            //high percent chance to paralyze
            int percentToParalyze = Random.Range(0, 100);
            if (percentToParalyze < 30)
            {
                print("Enemy has been paralyzed");
                enemyUnit[enemyUnitSelected].anim.enabled = false;
                enemyUnit[enemyUnitSelected].isParalyzed = true;
                enemyUnit[enemyUnitSelected].paralyzedCounter = 3;
            }
        }
    }

    public void FuseeDeVent()
    {
        GetAttackersMagAndPhys();

        for (int i = 0; i < enemyUnit.Count; i++)
        {
            enemyUnit[i].Charms = true;
        }
        attackAllEnemies = true;

        attackPower = 1.1f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }
    #endregion

    #region Dark Arts
    public void HomdomIritus()
    {
        GetAttackersMagAndPhys();

        for (int i = 0; i < enemyUnit.Count; i++)
        {
            enemyUnit[i].DA = true;
        }
        attackAllEnemies = true;

        attackPower = .6f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);

        if (successfulAttack)
        {
            for (int i = 0; i < enemyUnit.Count; i++)
            {
                //DecreaseDefense
                enemyUnit[i].defensePercent = .33f;
                enemyUnit[i].DefenseCounter = 3;
            }
        }
    }

    public void ManictomDotum()
    {
        GetAttackersMagAndPhys();
        enemyUnit[enemyUnitSelected].DA = true;

        attackPower = .6f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }

    public void OrbemPardonti()
    {
        GetAttackersMagAndPhys();
        enemyUnit[enemyUnitSelected].DA = true;

        attackPower = 1.1f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);

        if (successfulAttack)
        {
            //high percent chance to paralyze
            int percentToParalyze = Random.Range(0, 100);
            if (percentToParalyze < 60)
            {
                print("Enemy has been paralyzed");
                enemyUnit[enemyUnitSelected].anim.enabled = false;
                enemyUnit[enemyUnitSelected].isParalyzed = true;
                enemyUnit[enemyUnitSelected].paralyzedCounter = 3;
            }
        }
    }

    public void ScoptumTalli()
    {
        GetAttackersMagAndPhys();

        for (int i = 0; i < enemyUnit.Count; i++)
        {
            enemyUnit[i].DA = true;
        }
        attackAllEnemies = true;

        attackPower = 1.1f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);

        if (state == BattleState.Dani)
        {
            GameManager.DaniCurrentHP = 1;
            Dani.anim.SetTrigger("TakeDamage");
        }
        else if (state == BattleState.Erebus)
        {
            GameManager.ErebusCurrentHP = 1;
            Erebus.anim.SetTrigger("TakeDamage");
        }
        else if (state == BattleState.Phoebe)
        {
            GameManager.PhoebeCurrentHP = 1;
            Phoebe.anim.SetTrigger("TakeDamage");
        }
        else if (state == BattleState.Tristan)
        {
            GameManager.TristanCurrentHP = 1;
            Tristan.anim.SetTrigger("TakeDamage");
        }
        UpdateLifeUI();
    }
    #endregion

    #region Potions
    public void Charge()
    {
        //Need this drawn out
    }

    public void EssenceOfPride()
    {
        //Not quite done - need to tie in animation, MP lowering
        //IncreaseDefense
        Dani.defensePercent = -.25f;
        Dani.DefenseCounter = 3;
        Erebus.defensePercent = -.25f;
        Erebus.DefenseCounter = 3;
        Phoebe.defensePercent = -.25f;
        Phoebe.DefenseCounter = 3;
        Tristan.defensePercent = -.25f;
        Tristan.DefenseCounter = 3;

        //IncreaseAttack
        Dani.attackPercent = -.25f;
        Dani.AttackCounter = 3;
        Erebus.attackPercent = -.25f;
        Erebus.AttackCounter = 3;
        Phoebe.attackPercent = -.25f;
        Phoebe.AttackCounter = 3;
        Tristan.attackPercent = -.25f;
        Tristan.AttackCounter = 3;
    }

    public void Pestectus()
    {
        GetAttackersMagAndPhys();

        for (int i = 0; i < enemyUnit.Count; i++)
        {
            enemyUnit[i].Potions = true;
        }
        attackAllEnemies = true;

        damageToDo = 0;

        PostAttackItems(5);

        if (successfulAttack)
        {
            for (int i = 0; i < enemyUnit.Count; i++)
            {
                //Poisoned
                if (!enemyUnit[i].StrPo)
                    enemyUnit[i].isPoisoned = true;
            }
        }
    }

    public void PillarOfStrength()
    {
        //Something similar to this

       /* DaniPostAttackItems(0);

        if (successfulAttack)
        {
            print("Enemy has Defense Lowered");

            enemyUnit[enemyUnitSelected].defensePercent = -.5f;
            enemyUnit[enemyUnitSelected].DefenseCounter = 3;
        }*/
  /*  }*/
 /*
    public void PotionOfHealing()
    {
        //Need this drawn out
    }

    public void PotionOfResolve()
    {
        //Need this drawn out
    }

    public void PotionOfResurrection()
    {
        //Need this drawn out
    }
    #endregion

    #region Physical
    public void ForcefulLunge()
    {
        GetAttackersMagAndPhys();
        enemyUnit[enemyUnitSelected].Phys = true;

        attackPower = .6f;
        damageToDo = formulaicAttackBasePhys * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }
    public void PlayfulShove()
    {
        GetAttackersMagAndPhys();

        for (int i = 0; i < enemyUnit.Count; i++)
        {
            enemyUnit[i].Phys = true;
        }
        attackAllEnemies = true;

        attackPower = 1.1f;
        damageToDo = formulaicAttackBasePhys * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }
 
    public void VerticalSlice()
    {
        GetAttackersMagAndPhys();
        enemyUnit[enemyUnitSelected].Phys = true;

        attackPower = 1.1f;
        damageToDo = formulaicAttackBasePhys * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }
    #endregion

    #region Transfiguration

    public void ExpelindoElemicus()
    {
        GetAttackersMagAndPhys();
        enemyUnit[enemyUnitSelected].Trans = true;

        attackPower = .6f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }

    public void FierelloDeflectus()
    {
        GetAttackersMagAndPhys();
        enemyUnit[enemyUnitSelected].Trans = true;

        attackPower = 1.1f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }

    public void SellamTrahere()
    {
        GetAttackersMagAndPhys();

        for (int i = 0; i < enemyUnit.Count; i++)
        {
            enemyUnit[i].Trans = true;
        }
        attackAllEnemies = true;

        attackPower = 1.1f;
        damageToDo = formulaicAttackBaseMagic * attackPower * attackPercentOfUser * Random.Range(.95f, 1.1f);

        PostAttackItems(5);
    }
    #endregion

    void GetAttackersMagAndPhys()
    {
        if (state == BattleState.Dani)
        {
            currentAttackersMagic = GameManager.DaniMagic;
            currentAttackersPhys = GameManager.DaniPhysical;
            currentWandAttunement = GameManager.DaniWandAttunement;
            attackPercentOfUser = Dani.attackPercent;
            print("Dani attack percent" + Dani.attackPercent);
        }
        else if (state == BattleState.Erebus)
        {
            currentAttackersMagic = GameManager.ErebusMagic;
            currentAttackersPhys = GameManager.ErebusPhysical;
            currentWandAttunement = GameManager.ErebusWandAttunement;
            attackPercentOfUser = Erebus.attackPercent;
        }
        else if (state == BattleState.Phoebe)
        {
            currentAttackersMagic = GameManager.PhoebeMagic;
            currentAttackersPhys = GameManager.PhoebePhysical;
            currentWandAttunement = GameManager.PhoebeWandAttunement;
            attackPercentOfUser = Phoebe.attackPercent;
        }
        else if (state == BattleState.Tristan)
        {
            currentAttackersMagic = GameManager.TristanMagic;
            currentAttackersPhys = GameManager.TristanPhysical;
            currentWandAttunement = GameManager.TristanWandAttunement;
            attackPercentOfUser = Tristan.attackPercent;
        }
        else if (state == BattleState.EnemyTurn)
        {
             currentAttackersMagic = (GameManager.DaniMagic + GameManager.ErebusMagic + GameManager.PhoebeMagic + GameManager.TristanMagic) / 4.25f;
             currentAttackersPhys = (GameManager.DaniPhysical + GameManager.ErebusPhysical + GameManager.PhoebePhysical + GameManager.TristanPhysical) / 4.25f;
            currentWandAttunement = (GameManager.DaniWandAttunement + GameManager.ErebusWandAttunement + GameManager.PhoebeWandAttunement + GameManager.TristanWandAttunement) / 4.25f;
        }

        //FORMULA FOR ATTACKING
        //Magic
        // DamageToDo = ( ( (MagicDamage ^ 2.25f) * (WandAttunement ^ .85f) ) / 12 ) * AttackPower * AttackPercent * RandModifier (95% to 110%)
        //Phys
        // DamageToDo = ( ( (PhysDamage ^ 2.25f) * (WandAttunement ^ .85f) ) / 12 ) * AttackPower * AttackPercent * RandModifier (95% to 110%) 

        formulaicAttackBaseMagic = ((Mathf.Pow(currentAttackersMagic, 2.25f) * Mathf.Pow(currentWandAttunement, .85f)) / 12);
        formulaicAttackBasePhys = ((Mathf.Pow(currentAttackersPhys, 2.25f) * Mathf.Pow(currentWandAttunement, .85f)) / 12);

    }

    #endregion

    void RemoveCurrentEnemy()
    {
        print("this is where we tally experience");
        //totalExp += enemyUnit[enemyUnitSelected].ExperienceToDistribute;
        //totalExp += enemyUnit[enemyUnitSelected].ExperienceToDistribute;
        enemyCount--;

        enemySelectionParticle.transform.position = enemyBattleStationLocations[enemyUnitSelected].transform.position;
    }

    public void CreateMiniParticle()
    {
        print("create mini particle");
        StartCoroutine(CreateFullParticle());
    }

    IEnumerator CreateFullParticle()
    {
        yield return new WaitForSeconds(.25f);

        GameObject particle;
        if (state != BattleState.EnemyTurn)
        { //player attack
            if (!attackAllEnemies)
            {
                if (enemyUnit[enemyUnitSelected].Trans)
                    particle = Instantiate(transParticle, enemyUnit[enemyUnitSelected].transform.position + new Vector3(0, 1f, 0), enemyUnit[enemyUnitSelected].transform.rotation);
                else if (enemyUnit[enemyUnitSelected].Charms)
                    particle = Instantiate(charmsParticle, enemyUnit[enemyUnitSelected].transform.position + new Vector3(0, 1f, 0), enemyUnit[enemyUnitSelected].transform.rotation);
                else if (enemyUnit[enemyUnitSelected].Potions)
                    particle = Instantiate(potionsParticle, enemyUnit[enemyUnitSelected].transform.position + new Vector3(0, 1f, 0), enemyUnit[enemyUnitSelected].transform.rotation);
                else if (enemyUnit[enemyUnitSelected].Phys)
                    particle = Instantiate(physicalParticle, enemyUnit[enemyUnitSelected].transform.position + new Vector3(0, 1f, 0), enemyUnit[enemyUnitSelected].transform.rotation);
                else if (enemyUnit[enemyUnitSelected].DA)
                    particle = Instantiate(darkArtsParticle, enemyUnit[enemyUnitSelected].transform.position + new Vector3(0, 1f, 0), enemyUnit[enemyUnitSelected].transform.rotation);
                else if (enemyUnit[enemyUnitSelected].Ancient)
                    particle = Instantiate(ancientParticle, enemyUnit[enemyUnitSelected].transform.position + new Vector3(0, 1f, 0), enemyUnit[enemyUnitSelected].transform.rotation);
            }
            else
            {
                for (int i = 0; i < enemyUnit.Count; i++)
                {
                    if (enemyUnit[i].Trans)
                    {
                        for (int j = 0; j < enemyUnit.Count; j++)
                        {
                            particle = Instantiate(transParticle, enemyUnit[j].transform.position + new Vector3(0, 1f, 0), enemyUnit[j].transform.rotation);
                        }
                    }
                    else if (enemyUnit[i].Charms)
                    {
                        for (int j = 0; j < enemyUnit.Count; j++)
                        {
                            particle = Instantiate(charmsParticle, enemyUnit[j].transform.position + new Vector3(0, 1f, 0), enemyUnit[j].transform.rotation);
                        }
                    }
                    else if (enemyUnit[i].Potions)
                    {
                        for (int j = 0; j < enemyUnit.Count; j++)
                        {
                            particle = Instantiate(potionsParticle, enemyUnit[j].transform.position + new Vector3(0, 1f, 0), enemyUnit[j].transform.rotation);
                        }
                    }
                    else if (enemyUnit[i].Phys)
                    {
                        for (int j = 0; j < enemyUnit.Count; j++)
                        {
                            particle = Instantiate(physicalParticle, enemyUnit[j].transform.position + new Vector3(0, 1f, 0), enemyUnit[j].transform.rotation);
                        }
                    }
                    else if (enemyUnit[i].DA)
                    {
                        for (int j = 0; j < enemyUnit.Count; j++)
                        {
                            particle = Instantiate(darkArtsParticle, enemyUnit[j].transform.position + new Vector3(0, 1f, 0), enemyUnit[j].transform.rotation);
                        }
                    }
                    else if (enemyUnit[i].Ancient)
                    {
                        for (int j = 0; j < enemyUnit.Count; j++)
                        {
                            particle = Instantiate(ancientParticle, enemyUnit[j].transform.position + new Vector3(0, 1f, 0), enemyUnit[j].transform.rotation);
                        }
                    }
                    else if (enemyUnit[i].Ultimate)
                    {
                        particle = Instantiate(partyAttackParticle, new Vector3(1.37f, 0, 3.1f), Quaternion.identity);
                    }
                }
            }
           
        }
        else //enemy attack
        {
            if (!attackAllEnemies)
            {
                if (WhoToAttack == 0)
                {
                    if (Dani.Trans)
                        particle = Instantiate(transParticle, DaniBattleStation.transform.position + new Vector3(0, 1f, 0), DaniBattleStation.transform.rotation);
                    else if (Dani.Charms)
                        particle = Instantiate(charmsParticle, DaniBattleStation.transform.position + new Vector3(0, 1f, 0), DaniBattleStation.transform.rotation);
                    else if (Dani.Potions)
                        particle = Instantiate(potionsParticle, DaniBattleStation.transform.position + new Vector3(0, 1f, 0), DaniBattleStation.transform.rotation);
                    else if (Dani.Phys)
                        particle = Instantiate(physicalParticle, DaniBattleStation.transform.position + new Vector3(0, 1f, 0), DaniBattleStation.transform.rotation);
                    else if (Dani.DA)
                        particle = Instantiate(darkArtsParticle, DaniBattleStation.transform.position + new Vector3(0, 1f, 0), DaniBattleStation.transform.rotation);
                    else if (Dani.Ancient)
                        particle = Instantiate(ancientParticle, DaniBattleStation.transform.position + new Vector3(0, 1f, 0), DaniBattleStation.transform.rotation);
                }
                else if (WhoToAttack == 1)
                {
                    if (Erebus.Trans)
                        particle = Instantiate(transParticle, ErebusBattleStation.transform.position + new Vector3(0, 1f, 0), ErebusBattleStation.transform.rotation);
                    else if (Erebus.Charms)
                        particle = Instantiate(charmsParticle, ErebusBattleStation.transform.position + new Vector3(0, 1f, 0), ErebusBattleStation.transform.rotation);
                    else if (Erebus.Potions)
                        particle = Instantiate(potionsParticle, ErebusBattleStation.transform.position + new Vector3(0, 1f, 0), ErebusBattleStation.transform.rotation);
                    else if (Erebus.Phys)
                        particle = Instantiate(physicalParticle, ErebusBattleStation.transform.position + new Vector3(0, 1f, 0), ErebusBattleStation.transform.rotation);
                    else if (Erebus.DA)
                        particle = Instantiate(darkArtsParticle, ErebusBattleStation.transform.position + new Vector3(0, 1f, 0), ErebusBattleStation.transform.rotation);
                    else if (Erebus.Ancient)
                        particle = Instantiate(ancientParticle, ErebusBattleStation.transform.position + new Vector3(0, 1f, 0), ErebusBattleStation.transform.rotation);
                }
                else if (WhoToAttack == 2)
                {
                    if (Phoebe.Trans)
                        particle = Instantiate(transParticle, PhoebeBattleStation.transform.position + new Vector3(0, 1f, 0), PhoebeBattleStation.transform.rotation);
                    else if (Phoebe.Charms)
                        particle = Instantiate(charmsParticle, PhoebeBattleStation.transform.position + new Vector3(0, 1f, 0), PhoebeBattleStation.transform.rotation);
                    else if (Phoebe.Potions)
                        particle = Instantiate(potionsParticle, PhoebeBattleStation.transform.position + new Vector3(0, 1f, 0), PhoebeBattleStation.transform.rotation);
                    else if (Phoebe.Phys)
                        particle = Instantiate(physicalParticle, PhoebeBattleStation.transform.position + new Vector3(0, 1f, 0), PhoebeBattleStation.transform.rotation);
                    else if (Phoebe.DA)
                        particle = Instantiate(darkArtsParticle, PhoebeBattleStation.transform.position + new Vector3(0, 1f, 0), PhoebeBattleStation.transform.rotation);
                    else if (Phoebe.Ancient)
                        particle = Instantiate(ancientParticle, PhoebeBattleStation.transform.position + new Vector3(0, 1f, 0), PhoebeBattleStation.transform.rotation);
                }
                else if (WhoToAttack == 3)
                {
                    if (Tristan.Trans)
                        particle = Instantiate(transParticle, TristanBattleStation.transform.position + new Vector3(0, 1f, 0), TristanBattleStation.transform.rotation);
                    else if (Tristan.Charms)
                        particle = Instantiate(charmsParticle, TristanBattleStation.transform.position + new Vector3(0, 1f, 0), TristanBattleStation.transform.rotation);
                    else if (Tristan.Potions)
                        particle = Instantiate(potionsParticle, TristanBattleStation.transform.position + new Vector3(0, 1f, 0), TristanBattleStation.transform.rotation);
                    else if (Tristan.Phys)
                        particle = Instantiate(physicalParticle, TristanBattleStation.transform.position + new Vector3(0, 1f, 0), TristanBattleStation.transform.rotation);
                    else if (Tristan.DA)
                        particle = Instantiate(darkArtsParticle, TristanBattleStation.transform.position + new Vector3(0, 1f, 0), TristanBattleStation.transform.rotation);
                    else if (Tristan.Ancient)
                        particle = Instantiate(ancientParticle, TristanBattleStation.transform.position + new Vector3(0, 1f, 0), TristanBattleStation.transform.rotation);
                }
            }
            else
            {
                if ((Dani.Trans && !DaniDead) || (Erebus.Trans && !ErebusDead) || (Phoebe.Trans && !PhoebeDead) || (Tristan.Trans && !TristanDead))
                {
                    for (int j = 0; j < playerBattleStationLocations.Count; j++)
                    {
                        particle = Instantiate(transParticle, playerBattleStationLocations[j].transform.position + new Vector3(0, 1f, 0), playerBattleStationLocations[j].transform.rotation);
                    }
                }
                else if ((Dani.Charms && !DaniDead) || (Erebus.Charms && !ErebusDead) || (Phoebe.Charms && !PhoebeDead) || (Tristan.Charms && !TristanDead))
                {
                    for (int j = 0; j < playerBattleStationLocations.Count; j++)
                    {
                        particle = Instantiate(charmsParticle, playerBattleStationLocations[j].transform.position + new Vector3(0, 1f, 0), playerBattleStationLocations[j].transform.rotation);
                    }
                }
                if ((Dani.Potions && !DaniDead) || (Erebus.Potions && !ErebusDead) || (Phoebe.Potions && !PhoebeDead) || (Tristan.Potions && !TristanDead))
                {
                    for (int j = 0; j < playerBattleStationLocations.Count; j++)
                    {
                        particle = Instantiate(potionsParticle, playerBattleStationLocations[j].transform.position + new Vector3(0, 1f, 0), playerBattleStationLocations[j].transform.rotation);
                    }
                }
                if ((Dani.Phys && !DaniDead) || (Erebus.Phys && !ErebusDead) || (Phoebe.Phys && !PhoebeDead) || (Tristan.Phys && !TristanDead))
                {
                    for (int j = 0; j < playerBattleStationLocations.Count; j++)
                    {
                        particle = Instantiate(physicalParticle, playerBattleStationLocations[j].transform.position + new Vector3(0, 1f, 0), playerBattleStationLocations[j].transform.rotation);
                    }
                }
                if ((Dani.DA && !DaniDead) || (Erebus.DA && !ErebusDead) || (Phoebe.DA && !PhoebeDead) || (Tristan.DA && !TristanDead))
                {
                    for (int j = 0; j < playerBattleStationLocations.Count; j++)
                    {
                        particle = Instantiate(darkArtsParticle, playerBattleStationLocations[j].transform.position + new Vector3(0, 1f, 0), playerBattleStationLocations[j].transform.rotation);
                    }
                }
                if ((Dani.Ancient && !DaniDead) || (Erebus.Ancient && !ErebusDead) || (Phoebe.Ancient && !PhoebeDead) || (Tristan.Ancient && !TristanDead))
                {
                    for (int j = 0; j < playerBattleStationLocations.Count; j++)
                    {
                        particle = Instantiate(ancientParticle, playerBattleStationLocations[j].transform.position + new Vector3(0, 1f, 0), playerBattleStationLocations[j].transform.rotation);
                    }
                }
            }
        }
        yield return new WaitForSeconds(1f);

        if (state != BattleState.EnemyTurn)
        {
            if (enemyUnit[enemyUnitSelected].RefD || enemyUnit[enemyUnitSelected].RefT || enemyUnit[enemyUnitSelected].RefC || enemyUnit[enemyUnitSelected].RefPo || enemyUnit[enemyUnitSelected].RefPh || enemyUnit[enemyUnitSelected].RefA)
            {
                if (state == BattleState.Dani)
                {
                    bool reflectDead = Dani.TakeDamage(damageToDo);
                    if (reflectDead)
                    {
                        DaniAnim.SetBool("isDead", true);
                        DaniDead = true;
                    }
                        
                }
                if (state == BattleState.Erebus)
                {
                    bool reflectDead = Erebus.TakeDamage(damageToDo);
                    if (reflectDead)
                    {
                        ErebusAnim.SetBool("isDead", true);
                        ErebusDead = true;
                    }
                }
                if (state == BattleState.Phoebe)
                {
                    bool reflectDead = Phoebe.TakeDamage(damageToDo);
                    if (reflectDead)
                    {
                        PhoebeAnim.SetBool("isDead", true);
                        PhoebeDead = true;
                    }
                }
                if (state == BattleState.Tristan)
                {
                    bool reflectDead = Tristan.TakeDamage(damageToDo);
                    if (reflectDead)
                    {
                        TristanAnim.SetBool("isDead", true);
                        TristanDead = true;
                    }
                }
            }
            else
            {
                if (!attackAllEnemies)
                {
                    isDead = enemyUnit[enemyUnitSelected].TakeDamage(damageToDo * (1 + enemyUnit[enemyUnitSelected].defensePercent));

                    if (isDead)
                    {
                        RemoveCurrentEnemy();
                    }
                    else
                    {
                        if (enemyUnit[enemyUnitSelected].isDizzy)
                        {
                            enemyUnit[enemyUnitSelected].dizzyParticle.SetActive(true);
                        }

                        if (enemyUnit[enemyUnitSelected].isPoisoned && !enemyUnit[enemyUnitSelected].StrPo)
                        {
                            enemyUnit[enemyUnitSelected].poisonedDamage = (damageToDo / 3);
                            enemyUnit[enemyUnitSelected].poisonedTurnCount = 3;
                            enemyUnit[enemyUnitSelected].poisonedParticle.SetActive(true);
                        }

                        if (enemyUnit[enemyUnitSelected].DefenseCounter > 0)
                        {
                            enemyUnit[enemyUnitSelected].DefenseCounter -= 1;
                        }
                        if (enemyUnit[enemyUnitSelected].DefenseCounter <= 0)
                        {
                            enemyUnit[enemyUnitSelected].defensePercent = 0;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < enemyUnit.Count; i++)
                    {
                        isDead = enemyUnit[i].TakeDamage(damageToDo);

                        if (isDead)
                        {
                            RemoveCurrentEnemy();
                        }

                        if (enemyUnit[i].DefenseCounter > 0)
                        {
                            enemyUnit[i].DefenseCounter -= 1;
                        }
                        if (enemyUnit[i].DefenseCounter <= 0)
                        {
                            enemyUnit[i].defensePercent = 0;
                        }
                    }
                }
            }
        }
        else
        {
            //This would be reflecting attacks - but currently, no players have reflection, no need for reflecting
            if (!attackAllEnemies)
            {
                if (Dani.isDizzy)
                    Dani.dizzyParticle.SetActive(true);
                else if (Erebus.isDizzy)
                    Erebus.dizzyParticle.SetActive(true);
                else if (Phoebe.isDizzy)
                    Phoebe.dizzyParticle.SetActive(true);
                else if (Tristan.isDizzy)
                    Tristan.dizzyParticle.SetActive(true);

                if (Dani.isPoisoned && !Dani.StrPo)
                {
                    Dani.poisonedDamage = (damageToDo / 3);
                    Dani.poisonedTurnCount = 3;
                    Dani.poisonedParticle.SetActive(true);
                }
                else if (Erebus.isPoisoned && !Erebus.StrPo)
                {
                    Erebus.poisonedDamage = (damageToDo / 3);
                    Erebus.poisonedTurnCount = 3;
                    Erebus.poisonedParticle.SetActive(true);
                }
                else if (Phoebe.isPoisoned && !Phoebe.StrPo)
                {
                    Phoebe.poisonedDamage = (damageToDo / 3);
                    Phoebe.poisonedTurnCount = 3;
                    Phoebe.poisonedParticle.SetActive(true);
                }
                else if (Tristan.isPoisoned && !Tristan.StrPo)
                {
                    Tristan.poisonedDamage = (damageToDo / 3);
                    Tristan.poisonedTurnCount = 3;
                    Tristan.poisonedParticle.SetActive(true);
                }

                if (WhoToAttack == 0)
                {
                    isDead = Dani.TakeDamage(damageToDo * (1 + Dani.defensePercent));
                    if (isDead)
                    {
                        DaniAnim.SetBool("isDead", true);
                        DaniDead = true;
                    }
                    else
                        DaniAnim.SetTrigger("TakeDamage");
                    if (Dani.DefenseCounter > 0)
                        Dani.DefenseCounter -= 1;
                    if (Dani.DefenseCounter <= 0)
                        Dani.defensePercent = 0;
                }

                else if (WhoToAttack == 1)
                {
                    isDead = Erebus.TakeDamage(damageToDo * (1 + Erebus.defensePercent));
                    if (isDead)
                    {
                        ErebusAnim.SetBool("isDead", true);
                        ErebusDead = true;
                    }
                    else
                        ErebusAnim.SetTrigger("TakeDamage");
                    if (Erebus.DefenseCounter > 0)
                        Erebus.DefenseCounter -= 1;
                    if (Erebus.DefenseCounter <= 0)
                        Erebus.defensePercent = 0;
                }

                else if (WhoToAttack == 2)
                {
                    isDead = Phoebe.TakeDamage(damageToDo * (1 + Phoebe.defensePercent));
                    if (isDead)
                    {
                        PhoebeAnim.SetBool("isDead", true);
                        PhoebeDead = true;
                    }
                    else
                        PhoebeAnim.SetTrigger("TakeDamage");
                    if (Phoebe.DefenseCounter > 0)
                        Phoebe.DefenseCounter -= 1;
                    if (Phoebe.DefenseCounter <= 0)
                        Phoebe.defensePercent = 0;
                }

                else if (WhoToAttack == 3)
                {
                    isDead = Tristan.TakeDamage(damageToDo * (1 + Tristan.defensePercent));
                    if (isDead)
                    {
                        TristanAnim.SetBool("isDead", true);
                        TristanDead = true;
                    }
                    else
                        TristanAnim.SetTrigger("TakeDamage");
                    if (Tristan.DefenseCounter > 0)
                        Tristan.DefenseCounter -= 1;
                    if (Tristan.DefenseCounter <= 0)
                        Tristan.defensePercent = 0;
                }
            }
        }

        //This checks to see if the Enemy is Dead or has HP remaining
        if (isDead && (state != BattleState.EnemyTurn))
        {
            print("Enemy is now dead");
            if(state != BattleState.EnemyTurn)
                RemoveCurrentEnemy();
        }
        StartCoroutine(WaitForEndOfTurn());
    }

    IEnumerator WaitForEndOfTurn()
    {
        UpdateLifeUI();

        yield return new WaitForSeconds(2f);

        ClearEnemyAttributes();
        NextTurn();
    }

    void ClearEnemyAttributes()
    {
        damageToDo = 0;

        if (!attackAllEnemies)
        {
            enemyUnit[enemyUnitSelected].Trans = false;
            enemyUnit[enemyUnitSelected].Charms = false;
            enemyUnit[enemyUnitSelected].Potions = false;
            enemyUnit[enemyUnitSelected].DA = false;
            enemyUnit[enemyUnitSelected].Ancient = false;
            enemyUnit[enemyUnitSelected].Phys = false;

            Dani.Trans = false;
            Dani.Charms = false;
            Dani.Potions = false;
            Dani.DA = false;
            Dani.Ancient = false;
            Dani.Phys = false;

            Erebus.Trans = false;
            Erebus.Charms = false;
            Erebus.Potions = false;
            Erebus.DA = false;
            Erebus.Ancient = false;
            Erebus.Phys = false;

            Phoebe.Trans = false;
            Phoebe.Charms = false;
            Phoebe.Potions = false;
            Phoebe.DA = false;
            Phoebe.Ancient = false;
            Phoebe.Phys = false;

            Tristan.Trans = false;
            Tristan.Charms = false;
            Tristan.Potions = false;
            Tristan.DA = false;
            Tristan.Ancient = false;
            Tristan.Phys = false;

            successfulAttack = false;
        }
        else
        {
            if (state != BattleState.EnemyTurn)
            {
                for (int i = 0; i < enemyUnit.Count; i++)
                {
                    enemyUnit[i].Trans = false;
                    enemyUnit[i].Charms = false;
                    enemyUnit[i].Potions = false;
                    enemyUnit[i].DA = false;
                    enemyUnit[i].Ancient = false;
                    enemyUnit[i].Phys = false;
                    enemyUnit[i].Ultimate = false;
                }
            }
            else
            {
                Dani.Trans = false;
                Dani.Charms = false;
                Dani.Potions = false;
                Dani.DA = false;
                Dani.Ancient = false;
                Dani.Phys = false;

                Erebus.Trans = false;
                Erebus.Charms = false;
                Erebus.Potions = false;
                Erebus.DA = false;
                Erebus.Ancient = false;
                Erebus.Phys = false;

                Phoebe.Trans = false;
                Phoebe.Charms = false;
                Phoebe.Potions = false;
                Phoebe.DA = false;
                Phoebe.Ancient = false;
                Phoebe.Phys = false;

                Tristan.Trans = false;
                Tristan.Charms = false;
                Tristan.Potions = false;
                Tristan.DA = false;
                Tristan.Ancient = false;
                Tristan.Phys = false;
            }
            attackAllEnemies = false;
            successfulAttack = false;
        }
    }
}*/
