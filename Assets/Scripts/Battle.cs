using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public enum StateOfBattle { Start, WON, LOST }

public class Battle : MonoBehaviour
{
    public StateOfBattle state;

    public GameObject informationTextHolder, characterNameHolder;
    public TMP_Text informationText, characterNameText;

    public List<Transform> PlayerBattleStations = new List<Transform>();
    public List<Transform> EnemyBattleStations = new List<Transform>();

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

    public List<GameObject> deadCharacters = new List<GameObject>();

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

    public GameObject movingCamera;
    public GameObject playerUICanvas;
    public Transform AoEView;

    public string spellToStore;
    public int currentlySelectedEnemy;
    bool selectingOneTarget;

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
        state = StateOfBattle.Start;
    }
    IEnumerator SetUpBattle()
    {
        yield return new WaitForSeconds(0.25f);

        //This segment add players to the player list, enemies to the enemy list, and players/enemies to a combined 'combatants' list
        #region This Adds members to the Party as well as Sorts by Agility
        currentParty0 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[0]), PlayerBattleStations[0].transform.position + platformOffset, PlayerBattleStations[0].transform.rotation);
        Combatants.Add(currentParty0);
        playersInThisFight.Add(currentParty0);

        if (GameManager.inCurrentParty.Count > 0)
        {
            currentParty1 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[1]), PlayerBattleStations[1].transform.position + platformOffset, PlayerBattleStations[1].transform.rotation);
            Combatants.Add(currentParty1);
            playersInThisFight.Add(currentParty1);
        }
        if (GameManager.inCurrentParty.Count > 1)
        {
            currentParty2 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[2]), PlayerBattleStations[2].transform.position + platformOffset, PlayerBattleStations[2].transform.rotation);
            Combatants.Add(currentParty2);
            playersInThisFight.Add(currentParty2);
        }

        if (GameManager.inCurrentParty.Count > 2)
        {
            currentParty3 = Instantiate(Resources.Load<GameObject>(GameManager.inCurrentParty[3]), PlayerBattleStations[3].transform.position + platformOffset, PlayerBattleStations[3].transform.rotation);
            Combatants.Add(currentParty3);
            playersInThisFight.Add(currentParty3);
        }

        currentEnemy0 = Instantiate(Resources.Load<GameObject>(enemiesInThisFight[0]), EnemyBattleStations[0].transform.position + platformOffset, EnemyBattleStations[0].transform.rotation);
        Combatants.Add(currentEnemy0);
        enemiesInFight.Add(currentEnemy0);

        if (enemyCount > 1)
        {
            currentEnemy1 = Instantiate(Resources.Load<GameObject>(enemiesInThisFight[1]), EnemyBattleStations[1].transform.position + platformOffset, EnemyBattleStations[1].transform.rotation);
            Combatants.Add(currentEnemy1);
            enemiesInFight.Add(currentEnemy1);
        }
      
        if (enemyCount > 2)
        { 
            currentEnemy2 = Instantiate(Resources.Load<GameObject>(enemiesInThisFight[2]), EnemyBattleStations[2].transform.position + platformOffset, EnemyBattleStations[2].transform.rotation);
            Combatants.Add(currentEnemy2);
            enemiesInFight.Add(currentEnemy2);
        }
        if (enemyCount > 3)
        {
            currentEnemy3 = Instantiate(Resources.Load<GameObject>(enemiesInThisFight[3]), EnemyBattleStations[3].transform.position + platformOffset, EnemyBattleStations[3].transform.rotation);
            Combatants.Add(currentEnemy3);
            enemiesInFight.Add(currentEnemy3);
        }

        currentCombatantUnit = Combatants[currentCombatant].GetComponent<Unit>();
        //This stems from the overworld - the player attacked the enemy - player gets initiative, if enemy attacked player, enemy gets initiative
        if (GameManager.initiativeSiezedByEnemy)
        {
            for (int i = 0; i < enemiesInFight.Count; i++)
            {
                enemiesInFight[i].GetComponent<Unit>().tempAgility += 70;
                GameManager.initiativeSiezedByEnemy = false;
                StartCoroutine(ClearInformationText(2f, "Surprise Attack Enemies Attacking"));
            }
        }

        else
        {
            for (int i = 0; i < playersInThisFight.Count; i++)
            {
                playersInThisFight[i].GetComponent<Unit>().tempAgility += 70;
                StartCoroutine(ClearInformationText(2f, "Surprise Round"));
            }
        }

            Combatants = Combatants.OrderByDescending(x => (x.GetComponent<Unit>().Agility + x.GetComponent<Unit>().tempAgility)).ToList();
        #endregion

        UpdatePlayerHealthManaUI();
       // NextTurn();
    }

    private void Update()
    {
        if (selectingOneTarget)
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                for (int i = 0; i < enemiesInFight.Count; i++)
                {
                    enemiesInFight[i].GetComponent<Unit>().canvas.SetActive(false);
                    enemiesInFight[i].GetComponent<Unit>().targetParticle.SetActive(false);
                }

                currentlySelectedEnemy--;

                if (currentlySelectedEnemy < 0)
                    currentlySelectedEnemy = enemiesInFight.Count - 1;

            }


            if (Input.GetKeyDown(KeyCode.D))
            {
                for (int i = 0; i < enemiesInFight.Count; i++)
                {
                    enemiesInFight[i].GetComponent<Unit>().canvas.SetActive(false);
                    enemiesInFight[i].GetComponent<Unit>().targetParticle.SetActive(false);
                }

                currentlySelectedEnemy++;

                if (currentlySelectedEnemy >= enemiesInFight.Count)
                    currentlySelectedEnemy = 0;
            }
            
            enemiesInFight[currentlySelectedEnemy].GetComponent<Unit>().canvas.SetActive(true);
            enemiesInFight[currentlySelectedEnemy].GetComponent<Unit>().targetParticle.SetActive(true);          
        }
        else
        {
            print("Not selectingOneTarget ");
        }

        //Pressing Space advances the turn - this is not permenant - just for testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentCombatant = (currentCombatant + 1) % Combatants.Count;
            currentCombatantUnit = Combatants[currentCombatant].GetComponent<Unit>();
            NextTurn();
        }
        if (hasFled)
        {
            for (int i = 0; i < playersInThisFight.Count; i++)
            {
                if(!playersInThisFight[i].GetComponent<Unit>().characterIsDead)
                    playersInThisFight[i].transform.position += transform.right * 3 * Time.deltaTime;
            }
        }
    }

    void NextTurn()
    {
        //The combatants list is sorted in order from largest to smallest incrementing by one. 
        //When the list hits greater than the number of combatants, it re-sorts the list (trying to capture any agility changes in the turn) then circles back around to the largest
        if (currentCombatant >= Combatants.Count)
        {
            Combatants = Combatants.OrderByDescending(x => (x.GetComponent<Unit>().Agility + x.GetComponent<Unit>().tempAgility)).ToList();
            currentCombatant = 0;
            currentCombatantUnit = Combatants[currentCombatant].GetComponent<Unit>();
            for (int i = 0; i < Combatants.Count; i++)
            {
                Combatants[i].GetComponent<Unit>().tempAgility = 0;
            }
        }

        characterNameHolder.SetActive(true);
        characterNameText.text = currentCombatantUnit.Name;

        print("Currently Up: " + currentCombatantUnit.Name + " Agility: " + (currentCombatantUnit.Agility + currentCombatantUnit.tempAgility) + " CurrentHP: " + currentCombatantUnit.CurrentHP);
        currentCombatantUnit.TakingUnitTurn();
    }

    /// <summary>
    /// This section talks about the different support 'attacks' [isAHeal is derived from 'Unit']
    /// </summary>
    /// <param name="affectedCombatantList"></param>
    private void HandleSupportAttacks(List<GameObject> affectedCombatantList) {
        CastEssenceOfPride(affectedCombatantList);
        CastPestecus(affectedCombatantList);
        CastPillarsOfStrength();
        CastPotionOfHealing(affectedCombatantList);
        CastPotionOfResolve();
        CastPotionOfResurrection(affectedCombatantList);
    }



    /// <summary>
    /// 1) Brings one party member back to life 
    /// 2) Sets that party member's health to 50% 
    /// 3) adds them back into the turn order
    /// </summary>
    private void CastPotionOfResurrection(List<GameObject> affectedCombatantList) {
        PotionOfResurrection newPotion = new PotionOfResurrection();
        newPotion.ApplyEffect(deadCharacters[Random.Range(0, deadCharacters.Count)].GetComponent<Unit>());
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
        List<GameObject> sortedAffectedCombatantList = affectedCombatantList.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();
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
    /// Pestecus this slightly increases the health of all party members (that are alive)
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
    private void CastEssenceOfPride(List<GameObject> affectedCombatantList) {
        for (int i = 0; i < affectedCombatantList.Count; i++) {
            EssenceOfPride newCast = new EssenceOfPride();
            newCast.ApplyEffect(affectedCombatantList[i].GetComponent<Unit>());
        }
    }

    public void ResolvingATurnModified(float damage)
    {
        currentCombatantUnit.anim.SetTrigger("Attack");

        print($"{currentCombatantUnit.name} is using a cast type {currentCombatantUnit.attacks[currentCombatantUnit.randomAttack].castType}");

        if (currentCombatantUnit.isOnAuto)
        {
            //if you are attacking an enemy (not a support spell)
            if (currentCombatantUnit.attacks[currentCombatantUnit.randomAttack].castType == CastType.Enemy) {
                var currentTargetType = currentCombatantUnit.attacks[currentCombatantUnit.randomAttack].targetType;
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
            if (currentCombatantUnit.attacks[currentCombatantUnit.randomAttack].castType == CastType.Friendly) {
                HandleSupportAttacks(enemiesInFight);
            }
            //all of these are nestled in 'ResolvingATurn()'
        }
        else 
        {  
            //if you are attacking an enemy (not a support spell)
            if (currentCombatantUnit.attacks[currentCombatantUnit.randomAttack].castType == CastType.Enemy)
            {
                var currentTargetType = currentCombatantUnit.attacks[currentCombatantUnit.randomAttack].targetType;
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
            UpdatePlayerHealthManaUI();
        }
        //At the end of the turn, this cleans up/closes out any unnecessary value for all combatants
        for (int i = 0; i < Combatants.Count; i++)
        {
            Combatants[i].GetComponent<Unit>().CleanUp();
        }
    }

    private void HandleManualAoeAttack(float damage) {
        if (currentCombatantUnit.attacks[currentCombatantUnit.randomAttack].targetType == TargetType.AOE) {
            for (int i = 0; i < enemiesInFight.Count; i++)
                enemiesInFight[i].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.Finesse + currentCombatantUnit.FinesseEquipment));
        }
    }

    private void HandleManualSingleTargetAttack(float damage) {
        Combatants[currentCombatant].transform.LookAt(Combatants[currentlySelectedEnemy].transform);
        Combatants[currentlySelectedEnemy].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.Finesse + currentCombatantUnit.FinesseEquipment));
        print("single target");
    }

    private void HandleAutoAoeAttack(float damage) {
        if (currentCombatantUnit.isAPlayer) {
            for (int i = 0; i < enemiesInFight.Count; i++) {
                enemiesInFight[i].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.Finesse + currentCombatantUnit.FinesseEquipment));
            }
        } else {
            for (int i = 0; i < playersInThisFight.Count; i++) {
                if (!playersInThisFight[i].GetComponent<Unit>().characterIsDead) {
                    playersInThisFight[i].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.Finesse + currentCombatantUnit.FinesseEquipment));
                }
            }
        }
    }

    private void HandleAutoSingleTargetAttack(float damage) {
        if (currentCombatantUnit.isAPlayer) {
            enemiesInFight = enemiesInFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();
            int randAttack = Random.Range(0, 100);
            if (randAttack < 50) {
                Combatants[currentCombatant].transform.LookAt(enemiesInFight[0].transform);
                enemiesInFight[0].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.Finesse + currentCombatantUnit.FinesseEquipment));
            } else {
                int opponentToAttack = Random.Range(0, enemiesInFight.Count);
                enemiesInFight[opponentToAttack].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.Finesse + currentCombatantUnit.FinesseEquipment));
                Combatants[currentCombatant].transform.LookAt(enemiesInFight[opponentToAttack].transform);
            }
        } else {
            //i think this line can go away, it is to sort player's health if we want to structure it similar to the player where they target the weakest player
            // playersInThisFight = playersInThisFight.OrderBy(x => x.GetComponent<Unit>().CurrentHP).ToList();

            for (int i = 0; i < playersInThisFight.Count; i++) {
                if (!playersInThisFight[i].GetComponent<Unit>().characterIsDead)
                    playerToAttack++;
            }

            int playerToChoose = Random.Range(0, playersInThisFight.Count - 1);
            playersInThisFight[playerToChoose].GetComponent<Unit>().DidAttackKillCharacter(damage, (currentCombatantUnit.Finesse + currentCombatantUnit.FinesseEquipment));
            Combatants[currentCombatant].transform.LookAt(playersInThisFight[playerToChoose].transform);
            tempStoreOfPlayer = playersInThisFight[playerToChoose].GetComponent<Unit>().name;
            playerToAttack = 0;
        }
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

        if (playerCountForKnockedOut == playersInThisFight.Count && !currentCombatantUnit.isAPlayer && enemiesInFight.Count >= 2)
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

        if (enemyCountForKnockedOut == enemiesInFight.Count && currentCombatantUnit.isAPlayer && playersInThisFight.Count >= 2)
        {
            print("Enemy All Out Attack");

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

        for (int i = 0; i < enemiesInFight.Count; i++)
        {
            if (enemiesInFight[i].GetComponent<Unit>().hasBeenKnockedDown)
                chanceToLeave += 25;
        }

        if (percentToLeave <= chanceToLeave + currentCombatantUnit.Agility + currentCombatantUnit.AgilityEquipment)
        {
            //player flees
            for (int i = 0; i < playersInThisFight.Count; i++)
            {
                if (!playersInThisFight[i].GetComponent<Unit>().characterIsDead)
                {
                    playersInThisFight[i].transform.Rotate(0,180,0);
                    playersInThisFight[i].GetComponent<Unit>().anim.SetBool("isRunning", true);
                    hasFled = true;
                }
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
        currentCombatantUnit.defenseMultiplier = .5f;
        currentCombatantUnit.defenseBonusTurnCount = 2;
        currentCombatantUnit.CurrentHP += 3;
        currentCombatantUnit.CurrentMP += 3;
        if (currentCombatantUnit.CurrentHP >= currentCombatantUnit.MaxHP)
            currentCombatantUnit.CurrentHP = currentCombatantUnit.MaxHP;
        if (currentCombatantUnit.CurrentMP >= currentCombatantUnit.MaxMP)
            currentCombatantUnit.CurrentMP = currentCombatantUnit.MaxMP;

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
            var playerInstance = playersInThisFight[i];
            if (playerInstance.GetComponent<Unit>().characterIsDead)
            {
                if (!deadCharacters.Contains(playerInstance)) {
                    deadCharacters.Add(playerInstance);
                }
            }
        }

        //Current structure is to not remove players from the PlayersInThisFight list as they can be called back
        //So right now, if the number of deadPlayers equals the number of players in the fight - everyone has been killed and the player loses
        if (deadCharacters.Count >= playersInThisFight.Count)
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
            for (int i = 0; i < playersInThisFight.Count; i++)
            {
                if (currentCombatantUnit.name == playersInThisFight[i].GetComponent<Unit>().name)
                {
                    movingCamera.transform.position = PlayerBattleStations[i].GetChild(0).transform.position;
                    movingCamera.transform.rotation = PlayerBattleStations[i].GetChild(0).transform.rotation;
                }
            }
        }
        else
        {
            playerUICanvas.SetActive(false);

            //THIS IS A PROBLEM - ENEMIES CAN BE NAMED THE SAME THING
            for (int i = 0; i < enemiesInFight.Count; i++)
            {
                if (currentCombatantUnit.name == enemiesInFight[i].GetComponent<Unit>().name)
                {
                    movingCamera.transform.position = EnemyBattleStations[i].GetChild(0).transform.position;
                    movingCamera.transform.rotation = EnemyBattleStations[i].GetChild(0).transform.rotation;
                }
            }      
        }       
    }

    public void RotateCamera()
    {
        if (currentCombatantUnit.attacks[currentCombatantUnit.randomAttack].targetType == TargetType.SingleTarget)
        {
            for (int i = 0; i < playersInThisFight.Count; i++)
            {
                if (tempStoreOfPlayer == playersInThisFight[i].GetComponent<Unit>().name)
                {
                    movingCamera.transform.LookAt(playersInThisFight[i].transform);
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
        for (int i = 0; i < currentCombatantUnit.attacks.Count; i++)
        {
            if (currentCombatantUnit.attacks[i].ToString().Contains(spellToStore))
            {
                if (currentCombatantUnit.attacks[i].targetType == TargetType.SingleTarget)
                    selectingOneTarget = true;
                else
                {
                    for (int j = 0; j < enemiesInFight.Count; j++)
                    {
                        enemiesInFight[j].GetComponent<Unit>().canvas.SetActive(false);
                        enemiesInFight[j].GetComponent<Unit>().targetParticle.SetActive(false);
                    }
                    TurnOnAllEnemyTargets();
                }
            }
        }
    }

    void TurnOnAllEnemyTargets()
    {
        for (int i = 0; i < enemiesInFight.Count; i++)
        {
            enemiesInFight[i].GetComponent<Unit>().canvas.SetActive(true);
            enemiesInFight[i].GetComponent<Unit>().targetParticle.SetActive(true);
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
        currentCombatant = (currentCombatant + 1) % Combatants.Count;
        NextTurn();
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
        currentCombatant = (currentCombatant + 1) % Combatants.Count;
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
            currentCombatantUnit.anim.SetTrigger("Victory");
            currentCombatant = 0;
            currentCombatantUnit = Combatants[currentCombatant].GetComponent<Unit>();
            print("Player Won");
        }
    }

    //This clears any leftover data from GameManager picked up from 'RoamingMonster.cs' or weird data from the fight
    void ClearGameManager()
    {
        GameManager.initiativeSiezedByEnemy = false;
        GameManager.enemyCount = 0;
        GameManager.enemiesInThisFight.Clear();
    }

}
