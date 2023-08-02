using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using Character.Stats;

public enum StateOfBattle { Start, WON, LOST }

public class Battle : MonoBehaviour
{
    public StateOfBattle state;

    //check for which setup to use
    public bool usesSetup2;

    bool unitCurrentlyTakingTurn;

    public GameObject informationTextHolder, characterNameHolder;
    public TMP_Text informationText, characterNameText;

    public List<StationController> PlayerBattleStations = new List<StationController>();
    public List<StationController> EnemyBattleStations = new List<StationController>();

    GameObject currentParty0, currentParty1, currentParty2, currentParty3;
    GameObject currentEnemy0, currentEnemy1, currentEnemy2, currentEnemy3;


    public bool floor1 = true, floor2, floor3;
    public List<string> enemiesFloor1 = new List<string>();
    public List<string> enemiesFloor2 = new List<string>();
    public List<string> enemiesFloor3 = new List<string>();

    public List<string> enemiesInThisFight = new List<string>();
    public int enemyCount;

    public List<GameObject> Combatants = new List<GameObject>();
/*    public List<GameObject> playersInThisFight = new List<GameObject>();
    public List<GameObject> enemiesInFight = new List<GameObject>();

    public List<GameObject> deadCharacters = new List<GameObject>();*/

    float totalExperienceAwarded;
    public int currentCombatant = 0;

    public Unit currentCombatantUnit;

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
    float chanceToLeave;
    bool hasFled;
    string tempStoreOfPlayer;

    public GameObject movingCamera, cameraView;
    public GameObject playerUICanvas;
    public Transform AoEView, AllEnemiesKnockedDownPOV;

    public AbilityBase selectedSpell;
    public int currentlySelectedEnemy;
    bool selectingOneTarget;
    public GameObject victoryMenu;
    public bool groupAttackHappening;
    public GameObject damageParticle;

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

        if(usesSetup2)
            StartCoroutine(SetUpBattle2());
        else
            StartCoroutine(SetUpBattle());
        
        state = StateOfBattle.Start;
    }

    IEnumerator SetUpBattle2()
    {
        yield return new WaitForSeconds(0.25f);

        //iterate through each player in your party to add to the list
        //spot index is for where party will be placed

        int spotIndex = 0;
        foreach(string member in GameManager.inCurrentParty)
        {
            GameObject tempMember = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[spotIndex]));

            PlayerBattleStations[spotIndex].AddUnit(tempMember.GetComponent<Unit>());

            Combatants.Add(tempMember);
            spotIndex++;
        }

        
        for (int i = 0; i<enemyCount;i++)
        {
            GameObject tempEnemy = Instantiate(Resources.Load<GameObject>(enemiesInThisFight[i]));

            EnemyBattleStations[i].AddUnit(tempEnemy.GetComponent<Unit>());

            Combatants.Add(tempEnemy);
        }
        currentCombatantUnit = Combatants[currentCombatant].GetComponent<Unit>();

        OrderCombatants();
    }
    IEnumerator SetUpBattle()
    {
        yield return new WaitForSeconds(0.25f);
        UpdatePlayerHealthManaUI();
       // NextTurn();
    }
    public List<GameObject> GetEnemies()
    {
        return Combatants.Where(x => x.GetComponent<Unit>().isAPlayer == false ).ToList();
    }
    public List<GameObject> GetPlayers()
    {
        return Combatants.Where(x => x.GetComponent<Unit>().isAPlayer == true && x.GetComponent<Unit>().currentState != Unit.UnitState.Unconscious).ToList();
    }

    public List<GameObject> GetUnconciousPlayers()
    {
        return GetPlayers().Where(x => x.GetComponent<Unit>().currentState == Unit.UnitState.Unconscious).ToList();
    }

    public GameObject GetLowestHealthEnemy()
    {
        List<GameObject> enemies = GetEnemies();
        enemies.OrderBy(x => x.GetComponent<Health>().GetCurrentHP()).ToList();
        return enemies[0];
    }

    public GameObject GetRandomEnemy()
    {
        List<GameObject> enemies = GetEnemies();
        if (enemies.Count == 0) {
            Debug.Log($"Error: No enemies but received query for random enemy");
        }
        return enemies.Count == 0 ? null : enemies[Random.Range(0,enemies.Count)];
    }
    public GameObject GetRandomPlayer()
    {
        List<GameObject> consciousPlayers = GetPlayers();
        if (consciousPlayers.Count == 0) {
            Debug.Log($"Error: No players conscious but received query for random player");
        }
        return consciousPlayers.Count == 0 ? null : consciousPlayers[Random.Range(0, consciousPlayers.Count)];
    }

    void OrderCombatants()
    {
        currentCombatantUnit = Combatants[currentCombatant].GetComponent<Unit>();
        //This stems from the overworld - the player attacked the enemy - player gets initiative, if enemy attacked player, enemy gets initiative

        foreach (GameObject combatant in Combatants)
        {
            Unit combatantUnitProperties = combatant.GetComponent<Unit>();
            BaseStats combatantUnitStats = combatant.GetComponent<BaseStats>();
            if (!combatantUnitProperties.isAPlayer && GameManager.initiativeSiezedByEnemy)
            {
                /* TODO: Add enhancement effect with this modifier
                 * combatantUnitStats.GetStat(Stat.Agility) += 70;
                 * */
                StartCoroutine(ClearInformationText(2f, "Surprise Attack Enemies Attacking"));
            }
            else if (combatantUnitProperties.isAPlayer && GameManager.initiativeSiezedByPlayer)
            {
                /* TODO: Add enhancement effect with this modifier
                 * combatantUnitStats.GetStat(Stat.Agility) += 70;
                 * */
                StartCoroutine(ClearInformationText(2f, "Surprise Round"));
            }
        }


        Combatants = Combatants.OrderByDescending(x => (x.GetComponent<BaseStats>().GetStat(Stat.Agility)) + x.GetComponent<BaseStats>().GetStat(Stat.Agility)).ToList();

        UpdatePlayerHealthManaUI();
    }

    private void Update()
    {
        if (selectingOneTarget)
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                for (int i = 0; i < GetEnemies().Count; i++)
                {
                    GetEnemies()[i].GetComponent<Unit>().canvas.SetActive(false);
                    GetEnemies()[i].GetComponent<Unit>().targetParticle.SetActive(false);
                }

                currentlySelectedEnemy--;

                if (currentlySelectedEnemy < 0)
                    currentlySelectedEnemy = GetEnemies().Count - 1;

            }


            if (Input.GetKeyDown(KeyCode.D))
            {
                for (int i = 0; i < GetEnemies().Count; i++)
                {
                    GetEnemies()[i].GetComponent<Unit>().canvas.SetActive(false);
                    GetEnemies()[i].GetComponent<Unit>().targetParticle.SetActive(false);
                }

                currentlySelectedEnemy++;

                if (currentlySelectedEnemy >= GetEnemies().Count)
                    currentlySelectedEnemy = 0;
            }
            
            GetEnemies()[currentlySelectedEnemy].GetComponent<Unit>().canvas.SetActive(true);
            GetEnemies()[currentlySelectedEnemy].GetComponent<Unit>().targetParticle.SetActive(true);          
        }
        #region CHEATS REMOVE ONCE TESTING IS FINISHED
        if (Input.GetKeyDown(KeyCode.K))
        {
            //knock down all enemies
            for (int i = 0; i < GetEnemies().Count; i++)
            {
                GetEnemies()[i].GetComponent<Unit>().hasBeenKnockedDown = true;
                GetEnemies()[i].GetComponent<Unit>().anim.SetBool("knockedDown", true);
            }
            CheckIfAllMembersKnockedDown();
        }

        //Pressing Space advances the turn - this is not permenant - just for testing
        if (Input.GetKeyDown(KeyCode.Space) && !unitCurrentlyTakingTurn)
        {
            currentCombatant = (currentCombatant + 1) % Combatants.Count;
            currentCombatantUnit = Combatants[currentCombatant].GetComponent<Unit>();

            if(currentCombatantUnit.GetComponent<Unit>().currentState != Unit.UnitState.Unconscious || currentCombatantUnit.GetComponent<Unit>().currentState != Unit.UnitState.Dead || currentCombatantUnit.GetComponent<Unit>().currentState != Unit.UnitState.Fled)
                StartCoroutine(NextTurn());
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            //this enters a victory state, TESTING, REMOVE EVENTUALLY
            state = StateOfBattle.WON;
            EndBattle();
        }
        #endregion
    }

    IEnumerator NextTurn()
    {
        //The combatants list is sorted in order from largest to smallest incrementing by one. 
        //When the list hits greater than the number of combatants, it re-sorts the list (trying to capture any agility changes in the turn) then circles back around to the largest
        if (currentCombatant >= Combatants.Count)
        {
            Combatants = Combatants.OrderByDescending(x => x.GetComponent<BaseStats>().GetStat(Stat.Agility) + x.GetComponent<BaseStats>().GetStat(Stat.Agility)).ToList();
            currentCombatant = 0;
            currentCombatantUnit = Combatants[currentCombatant].GetComponent<Unit>();
            /*
             * TODO : Modify ordering based on agility, without modifying base agiity so enhancements persist
                for (int i = 0; i < Combatants.Count; i++)
                {
                    Combatants[i].GetComponent<Unit>().Agility = 0;
                }
             */
        }

        characterNameHolder.SetActive(true);
        characterNameText.text = currentCombatantUnit.Name;
        int currentCharacterAgilityStat = currentCombatantUnit.GetComponent<BaseStats>().GetStat(Stat.Agility);
        int currentCharacterCurrentHP = currentCombatantUnit.GetComponent<Health>().GetCurrentHP();
        print("Currently Up: " + currentCombatantUnit.Name + " Agility: " + (currentCharacterAgilityStat) + " CurrentHP: " + currentCharacterCurrentHP);

        if (currentCombatantUnit.GetComponent<Unit>().currentState != Unit.UnitState.Unconscious)
        {
            unitCurrentlyTakingTurn= true;
            yield return StartCoroutine(currentCombatantUnit.TakingUnitTurn());

            UpdatePlayerHealthManaUI();
            Debug.LogWarning("finished turn");
            unitCurrentlyTakingTurn = false;
        }
            
    }

    /// <summary>
    /// This section talks about the different support 'attacks' [isAHeal is derived from 'Unit']
    /// </summary>
    /// <param name="affectedCombatantList"></param>
    private void HandleSupportAttacks(List<GameObject> affectedCombatantList, AttackBase selectedAttack) {
        // TODO : Refactor without string dependency
        switch (selectedAttack.name) {
            case "EssenceOfPride":
                //CastEssenceOfPride();
                break;
            default:
                break;
        }
        /*
        CastPestecus(affectedCombatantList);
        CastPillarsOfStrength();
        CastPotionOfHealing(affectedCombatantList);
        CastPotionOfResolve();
        CastPotionOfResurrection(affectedCombatantList);
        */
    }



    /// <summary>
    /// 1) Brings one party member back to life 
    /// 2) Sets that party member's hitpoints to 50% 
    /// 3) adds them back into the turn order
    /// </summary>
    private void CastPotionOfResurrection(List<GameObject> affectedCombatantList) {
        PotionOfResurrection newPotion = new PotionOfResurrection();
        newPotion.ApplyEffect(GetUnconciousPlayers()[Random.Range(0, GetUnconciousPlayers().Count)].GetComponent<Unit>());
    }
    /// <summary>
    /// Applies potion of resolve to current combatant unit
    /// </summary>
    private void CastPotionOfResolve() {
        PotionOfResolve newPotion = new PotionOfResolve();
        newPotion.ApplyEffect(currentCombatantUnit);
    }

    /// <summary>
    /// CastPotionOfHealing Heals one party member. There is an 80% chance they heal the weakest party member, 20% chance it randomly heals another person in the party
    /// </summary>
    private void CastPotionOfHealing(List<GameObject> affectedCombatantList) {
        List<GameObject> sortedAffectedCombatantList = affectedCombatantList.OrderBy(x => x.GetComponent<Health>().GetCurrentHP()).ToList();
        int chanceOfPersonToHeal = Random.Range(0, 100);
        Unit personToHeal;
        if (chanceOfPersonToHeal <= 80) { // 80% heal first unit
            personToHeal = sortedAffectedCombatantList[0].GetComponent<Unit>();
        } else { // 20% heal random
            personToHeal = sortedAffectedCombatantList[Random.Range(0, sortedAffectedCombatantList.Count)].GetComponent<Unit>();
        }
        PotionOfHealing newPotion = new PotionOfHealing();
        newPotion.ApplyEffect(personToHeal);
    }

    /// <summary>
    /// Pillar of Strength this increases the defense of the current caster by 50% for 3 turns
    /// </summary>
    private void CastPillarsOfStrength() {
        PillarOfStrength newCast = new PillarOfStrength();
        newCast.ApplyEffect(currentCombatantUnit);
       
    }
    /// <summary>
    /// Pestecus this slightly increases the hitpoints of all party members (that are alive)
    /// </summary>
    private void CastPestecus(List<GameObject> affectedCombatantList) {
        for (int i = 0; i < affectedCombatantList.Count; i++) {
            Pestecus newCast = new Pestecus();
            newCast.ApplyEffect(affectedCombatantList[i].GetComponent<Unit>());
        }
    }

    /// <summary>
    /// Essence Of Pride increases the party's attack by 50%, defense by 50%, and keeps those stats for 3 turns [this is hard coded to prevent stacking = 3 is different than += 3]
    /// </summary>
    private void CastEssenceOfPride() {
        // TODO : Get party members from game manager
        for (int i = 0; i < GetPlayers().Count; i++) {
            EssenceOfPride newCast = new EssenceOfPride();
            newCast.ApplyEffect(GetPlayers()[i].GetComponent<Unit>());
        }
    }

    public void ResolvingATurn(float damage, AttackBase selectedAttack)
    {

        currentCombatantUnit.anim.SetTrigger("Attack");

        print($"{currentCombatantUnit.name} is using a cast type {selectedAttack.castType}");

        if (currentCombatantUnit.isOnAuto)
        {
            selectedSpell = selectedAttack; // Need to set selectedspell here for later camera rotation
            //if you are attacking an enemy (not a support spell)
            if (selectedAttack.castType == CastType.Enemy) {
                var currentTargetType = selectedAttack.targetType;
                print($"AUTOUNIT: {currentCombatantUnit.name} is using target type ${currentTargetType}");
                switch(currentTargetType) {
                    case TargetType.SingleTarget:
                        HandleAutoSingleTargetAttack(damage);
                        break;
                    case TargetType.AOE:
                        HandleAutoAoeAttack(damage);
                        break;
                    default:
                        //
                        break;
                }
            }
            //if cast type support
            if (selectedAttack.castType == CastType.Friendly) {
                HandleSupportAttacks(GetEnemies(), selectedAttack);
            }
            //all of these are nestled in 'ResolvingATurn()'
        }
        else 
        {  
            //if you are attacking an enemy (not a support spell)
            if (selectedAttack.castType == CastType.Enemy)
            {
                var currentTargetType = selectedAttack.targetType;
                print($"MANUALUNIT: {currentCombatantUnit.name} is using target type ${currentTargetType}");
                switch(currentTargetType) {
                    case TargetType.SingleTarget:
                        HandleManualSingleTargetAttack(damage);
                        break;
                    case TargetType.AOE:
                        HandleManualAoeAttack(damage);
                        break;
                    default:
                        //
                        break;
                }
            }
            //if cast type support
            if (selectedAttack.castType == CastType.Friendly) {
                HandleSupportAttacks(GetEnemies(), selectedAttack);
            }
            UpdatePlayerHealthManaUI();
        }
        //At the end of the turn, this cleans up/closes out any unnecessary value for all combatants
        for (int i = 0; i < Combatants.Count; i++)
        {
            Combatants[i].GetComponent<Unit>().CleanUp();
        }
    }

    private void HandleManualAoeAttack(float damage) {
        for (int i = 0; i < GetEnemies().Count; i++) {
            //GetEnemies()[i].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.GetComponent<BaseStats>().GetStat(Stat.Finesse) + currentCombatantUnit.FinesseEquipment));
        }
    }

    private void HandleManualSingleTargetAttack(float damage) {
        Combatants[currentCombatant].transform.LookAt(Combatants[currentlySelectedEnemy].transform);
        //Combatants[currentlySelectedEnemy].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.GetComponent<BaseStats>().GetStat(Stat.Finesse) + currentCombatantUnit.FinesseEquipment));
    }

    private void HandleAutoAoeAttack(float damage) {
        if (currentCombatantUnit.isAPlayer) {
            for (int i = 0; i < GetEnemies().Count; i++) {
                //GetEnemies()[i].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.GetComponent<BaseStats>().GetStat(Stat.Finesse) + currentCombatantUnit.FinesseEquipment));
            }
        } else {
            
            //shouldn't need the if since get players only returns conscious ones
            for (int i = 0; i < GetPlayers().Count; i++) {
                //if (!GetPlayers()[i].GetComponent<Unit>().characterIsDead) {
                    //GetPlayers()[i].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.GetComponent<BaseStats>().GetStat(Stat.Finesse) + currentCombatantUnit.FinesseEquipment));
                //}
            }
        }
    }

    private void HandleAutoSingleTargetAttack(float damage) {
        if (currentCombatantUnit.isAPlayer) {
            //enemiesInFight = enemiesInFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();
            int randAttack = Random.Range(0, 100);
            if (randAttack < 50) {
                Combatants[currentCombatant].transform.LookAt(GetLowestHealthEnemy().transform);
                //GetLowestHealthEnemy().GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.GetComponent<BaseStats>().GetStat(Stat.Finesse) + currentCombatantUnit.FinesseEquipment));
            } else {
                int opponentToAttack = Random.Range(0, GetEnemies().Count);
                //GetEnemies()[opponentToAttack].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.GetComponent<BaseStats>().GetStat(Stat.Finesse) + currentCombatantUnit.FinesseEquipment));
                Combatants[currentCombatant].transform.LookAt(GetEnemies()[opponentToAttack].transform);
            }
        } else {
            //i think this line can go away, it is to sort player's hitpoints if we want to structure it similar to the player where they target the weakest player
            // playersInThisFight = playersInThisFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();

            for (int i = 0; i < GetPlayers().Count; i++) {
                //shouldn't need the if since get players only returns conscious ones
                //if (!GetPlayers()[i].GetComponent<Unit>().characterIsDead)
                playerToAttack++;
            }

            if (GetPlayers().Count > 0)
            {
                print(GetPlayers().Count);

                int playerToChoose = Random.Range(0, GetPlayers().Count - 1);

                Combatants[currentCombatant].transform.LookAt(GetPlayers()[playerToChoose].transform);
                //bool playerWasKilled = GetPlayers()[playerToChoose].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.GetComponent<BaseStats>().GetStat(Stat.Finesse) + currentCombatantUnit.FinesseEquipment));

                tempStoreOfPlayer = GetPlayers()[playerToChoose].GetComponent<Unit>().name;
                playerToAttack = 0;
            }
            else
            {
                Debug.LogWarning("All Players Unconcious");
            }
        }
    }


    public void CheckIfAllMembersKnockedDown()
    {
        //There is a system that *will be* integrated. If you hit someone with a 'critical' or 'weak', you will knock them down until their next turn. 
        //if you knock all the enemies (or players down) then you will launch an all-out attack
        //All Out Attack does damage to every enemy with a math formula similar to [(player1damage + player2damage + player3damage + player4damage) * .75f ]
        //all knocked down enemies are then no longer knocked down



        //If an enemy knocks down a player
        for (int i = 0; i < GetEnemies().Count; i++)
        {
            if (GetEnemies()[i].GetComponent<Unit>().hasBeenKnockedDown)
            {
                playerCountForKnockedOut++;
            }
        }

        if (playerCountForKnockedOut == GetPlayers().Count && !currentCombatantUnit.isAPlayer && GetEnemies().Count >= 2)
        {
            print("Enemy All Out Attack");

            //All players take damage based on who is still standing on the enemy side
            //Check if players are alive
            for (int i = 0; i < GetPlayers().Count; i++)
            {
                GetPlayers()[i].GetComponent<Unit>().anim.SetBool("knockedDown", false);
            }
            //clear playerCountForKnockedOut
        }
        //If a player knocks down an enemy
        for (int i = 0; i < GetEnemies().Count; i++)
        {
            if (GetEnemies()[i].GetComponent<Unit>().hasBeenKnockedDown)
            {
                enemyCountForKnockedOut++;
            }
        }

        if (enemyCountForKnockedOut == GetEnemies().Count && currentCombatantUnit.isAPlayer && GetPlayers().Count >= 2)
        {
            print("Player All Out Attack");

            //Open UI menu to let players know attack is available
            groupAttackHappening = true;

            movingCamera.transform.position = AllEnemiesKnockedDownPOV.transform.position;
            //button is handled on screen and triggered from button in PlayerUICanvas script


            //All players take damage based on who is still standing on the enemy side
            //Check if players are alive
            //players stand back up
            //clear enemyCountForKnockedOut
        }
    }

    //Player has a chance to flee if they press the 'flee option'. This is done by taking a random # 0-100. For each downed enemy, add 25, then add agility and equipment. If chanceToLeave<=sum, leave
    public void FleeChance()
    {
        int percentToLeave = Random.Range(0, 100);

        for (int i = 0; i < GetEnemies().Count; i++)
        {
            if (GetEnemies()[i].GetComponent<Unit>().hasBeenKnockedDown)
                chanceToLeave += 25;
        }

        if (percentToLeave <= chanceToLeave + currentCombatantUnit.GetComponent<BaseStats>().GetStat(Stat.Agility) + currentCombatantUnit.AgilityEquipment)
        {
            //player flees
            for (int i = 0; i < GetPlayers().Count; i++)
            {
                //if (!GetPlayers()[i].GetComponent<Unit>().characterIsDead)
                //{
                    GetPlayers()[i].transform.Rotate(0,180,0);
                    GetPlayers()[i].GetComponent<Unit>().anim.SetBool("isRunning", true);
                    hasFled = true;
                //}
            }
            chanceToLeave = 0;
            StartCoroutine(ClearInformationText(2f, "Party escaped"));
            StartCoroutine(BattleCleanUp());
        }
        else
        {
            chanceToLeave = 0;
            StartCoroutine(ClearInformationText(2f, "Unable to escape"));
        }
    }
    //This has the current player spend their turn guarding, increasing their defense, with a little recovery;
    public void GuardPlayer()
    {
        StartCoroutine(ClearInformationText(2f, "Guard"));
        Health currentCombatantHealth = currentCombatantUnit.GetComponent<Health>();
        currentCombatantUnit.defenseMultiplier = .5f;
        currentCombatantUnit.defenseBonusTurnCount = 2;
        currentCombatantHealth.AddHealth(3);
        currentCombatantUnit.GetComponent<Magic>().AddMagic(3);

        UpdatePlayerHealthManaUI();

        //At the end of the turn, this cleans up/closes out any unnecessary value for all combatants
        for (int i = 0; i < Combatants.Count; i++)
        {
            Combatants[i].GetComponent<Unit>().CleanUp();
        }
    }

    IEnumerator BattleCleanUp()
    {
        yield return new WaitForSeconds(2f);
        ExperienceAndDeathCollection();
        ClearGameManager();
    }

    IEnumerator ClearInformationText(float timer, string message)
    {
        informationTextHolder.SetActive(true);
        informationText.text = message;
        yield return new WaitForSeconds(timer);

        characterNameHolder.SetActive(false);
        characterNameText.text = "";
        informationText.text = "";
        informationTextHolder.SetActive(false);
    }

    public void ExperienceAndDeathCollection()
    {
        //Experience and Death - this tracks: 1) who is left in the fight 2) has the player won or lost 

        //This first part - if either a player or enemy is killed, they are removed from the Combatants list (their turn is skipped in battle)
        for (int i = 0; i < Combatants.Count; i++)
        {
            if (Combatants[i].GetComponent<Health>().GetCurrentHP() <= 0)
            {
                totalExperienceAwarded += Combatants[i].GetComponent<Unit>().experienceEarned;

                foreach (Transform child in Combatants[i].transform)
                {
                    child.gameObject.SetActive(false);
                }
                if (Combatants[i].GetComponent<Unit>().isAPlayer == false)
                {
                    Combatants.Remove(Combatants[i]);
                }
            }
        }

        //If an enemy is killed in battle, they are: 1) Removed from play 2) Removed from the list 3) their experience is added to a total number
        for (int i = 0; i < GetEnemies().Count; i++)
        {
            if (GetEnemies()[i].GetComponent<Health>().GetCurrentHP() <= 0)
            {
                totalExperienceAwarded += Combatants[i].GetComponent<Unit>().experienceEarned;
                GetEnemies().Remove(GetEnemies()[i]);
            }
        }

        //If there are no more enemies, the player has won
        if (GetEnemies().Count <= 0)
        {
            state = StateOfBattle.WON;
            EndBattle();
        }

        //Current structure is to not remove players from the PlayersInThisFight list as they can be called back
        //So right now, if the number of deadPlayers equals the number of players in the fight - everyone has been killed and the player loses
        if (GetUnconciousPlayers().Count >= GetPlayers().Count)
        {
            state = StateOfBattle.LOST;
            EndBattle();
        }
    }

    public void MoveCamera()
    {
        if (currentCombatantUnit.isAPlayer)
        {
            playerUICanvas.SetActive(true);
            for (int i = 0; i < GetPlayers().Count; i++)
            {
                if (currentCombatantUnit.name == GetPlayers()[i].GetComponent<Unit>().name)
                {
                    movingCamera.transform.position = PlayerBattleStations[i].perspectiveCamera.position;
                    movingCamera.transform.rotation = PlayerBattleStations[i].perspectiveCamera.rotation;
                }
            }
        }
        else
        {
            playerUICanvas.SetActive(false);

            //THIS IS A PROBLEM - ENEMIES CAN BE NAMED THE SAME THING
            for (int i = 0; i < GetEnemies().Count; i++)
            {
                if (currentCombatantUnit.name == GetEnemies()[i].GetComponent<Unit>().name)
                {
                    movingCamera.transform.position = EnemyBattleStations[i].perspectiveCamera.position;
                    movingCamera.transform.rotation = EnemyBattleStations[i].perspectiveCamera.rotation;
                }
            }      
        }       
    }

    public void RotateCamera()
    {
        if (selectedSpell.targetType == TargetType.SingleTarget) // TODO: Modify selectedSpell to not use global, this may be inaccurate during autoplay
        {
            for (int i = 0; i < GetPlayers().Count; i++)
            {
                if (tempStoreOfPlayer == GetPlayers()[i].GetComponent<Unit>().name)
                {
                    movingCamera.transform.LookAt(GetPlayers()[i].transform);
                }
            }
            tempStoreOfPlayer = "";
        }
        else
        {
            movingCamera.transform.position = AoEView.transform.position;
            movingCamera.transform.rotation = AoEView.rotation;
        }

    }

    public void TurnOnIndividualAttackItems()
    {
        //move camera
        //turn on particle system (based on attack)
        for (int i = 0; i < currentCombatantUnit.abilities.Count; i++)
        {
            if (currentCombatantUnit.abilities[i] == selectedSpell)
            {
                if (currentCombatantUnit.abilities[i].targetType == TargetType.SingleTarget)
                    selectingOneTarget = true;
                else
                {
                    for (int j = 0; j < GetEnemies().Count; j++)
                    {
                        GetEnemies()[j].GetComponent<Unit>().canvas.SetActive(false);
                        GetEnemies()[j].GetComponent<Unit>().targetParticle.SetActive(false);
                    }
                    TurnOnAllEnemyTargets();
                }
            }
        }
    }

    void TurnOnAllEnemyTargets()
    {
        for (int i = 0; i < GetEnemies().Count; i++)
        {
            GetEnemies()[i].GetComponent<Unit>().canvas.SetActive(true);
            GetEnemies()[i].GetComponent<Unit>().targetParticle.SetActive(true);
        }
    }
    public void TurnOffIndividualAttackItems()
    {
        //return camera
        //turn off particle system
 //       selectingOneTarget = false;
    }

    public void AdvanceTurn()
    {
        //currentCombatant = (currentCombatant + 1) % Combatants.Count;
        //NextTurn();
    }

    public void UpdatePlayerHealthManaUI()
    {
        //This updates the player's UI in the bottom right hand corner (usually done at the end of the player's turn)
        for (int i = 0; i < GetPlayers().Count; i++)
        {
            //If player's hitpoints < 0 their icons are turned black
            //This is hardcoded to be at 0 so the player doesn't see how negative their hitpoints is
            if (GetPlayers()[i].GetComponent<Health>().GetCurrentHP() <= 0)
            {
                healthText[i].text = "0";
                healthUI[i].value = 0;

                triangles[i].color = Color.black;
                icons[i].color = Color.black;
            }

            //otherwise, the stats are updated to reflected current state
            else
            {
                healthUI[i].value = GetPlayers()[i].GetComponent<Health>().GetCurrentHP() / GetPlayers()[i].GetComponent<Health>().GetMaxHP();
                healthText[i].text = GetPlayers()[i].GetComponent<Health>().GetCurrentHP().ToString("F0");

                triangles[i].color = GameManager.colorsInParty[i];
                icons[i].color = Color.white;
                icons[i].sprite = Resources.Load<Sprite>("PlayerIcons/" + GetPlayers()[i].GetComponent<Unit>().Name);
            }
            manaUI[i].value = GetPlayers()[i].GetComponent<Magic>().GetCurrentMP() / GetPlayers()[i].GetComponent<Magic>().GetMaxMP();
            manaText[i].text = GetPlayers()[i].GetComponent<Magic>().GetCurrentMP().ToString("F0");
        }
    }
    public void EndTurn()
    {
        //this ends the turn and advances to the next one
        //currentCombatant = (currentCombatant + 1) % Combatants.Count;
        //NextTurn();
    }

    void EndBattle()
    {
        playerUICanvas.SetActive(false);
        ClearGameManager();
        //this just prints states, but will be used for triggering Experience windows (if won) or losing screen (if lost)
        if (state == StateOfBattle.LOST)
        {
            print("Player Lost");

        }
        else
        {
            currentCombatantUnit.anim.SetTrigger("Victory");

            for (int i = 0; i < GetPlayers().Count; i++)
            {
                //shouldn't need the if since get players only returns conscious ones
                //if (!GetPlayers()[i].GetComponent<Unit>().characterIsDead)
                //{
                GetPlayers()[i].GetComponent<Unit>().anim.SetTrigger("battleWon");

                    //Change colors of all materials to match party member color
                    GetPlayers()[i].GetComponent<Unit>().objectToChangeMaterialOf.GetComponent<Renderer>().materials = GetPlayers()[i].GetComponent<Unit>().playerMaterial;
                //}
            }

            movingCamera.transform.position = Combatants[currentCombatant].GetComponent<Unit>().victoryPlacement.transform.position;
            movingCamera.transform.rotation = Combatants[currentCombatant].GetComponent<Unit>().victoryPlacement.transform.rotation;
            cameraView.GetComponent<Animator>().SetTrigger("victory");


            //open experience menu
            StartCoroutine(OpenVictoryMenu());
            //open menu to return to lobby
            currentCombatant = 0;
            currentCombatantUnit = Combatants[currentCombatant].GetComponent<Unit>();
            print("Player Won");
        }
    }

    IEnumerator OpenVictoryMenu()
    {
        yield return new WaitForSeconds(1f);
        victoryMenu.SetActive(true);
    }

    //This clears any leftover data from GameManager picked up from 'RoamingMonster.cs' or weird data from the fight
    void ClearGameManager()
    {
        GameManager.initiativeSiezedByEnemy = false;
        GameManager.enemyCount = 0;
        GameManager.enemiesInThisFight.Clear();
    }

}
