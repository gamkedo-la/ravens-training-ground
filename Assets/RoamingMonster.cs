using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoamingMonster : MonoBehaviour
{
    int enemyCount;
    public List<string> enemiesInThisFight = new List<string>();
    public List<string> enemiesFloor1 = new List<string>();

    public List<GameObject> enemiesInThisList = new List<GameObject>();

    int totalLevel;
    float avgEnemyLevelPerEnemyInParty;

    bool isInArea;

    public GameObject bparticle, gparticle, rparticle;

    public string sceneToLoad;

    // Start is called before the first frame update
    void Start()
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
            enemiesInThisList.Add(Resources.Load<GameObject>(enemiesInThisFight[i]));

            totalLevel += enemiesInThisList[i].GetComponent<Unit>().CurrentLevel;
        } 
    }

    private void Update()
    {
        //If the player is colliding with a wandering enemy and presses 'Space', the GameManager is populated with data to pass to 'Battle.cs', then the Battle scene is loaded.
        if (Input.GetKeyDown(KeyCode.Space) && isInArea)
        {
            GameManager.enemyCount = 0;
            GameManager.enemiesInThisFight.Clear();

            GameManager.enemyCount = enemyCount;

            for (int i = 0; i < enemiesInThisFight.Count; i++)
                GameManager.enemiesInThisFight.Add(enemiesInThisFight[i]);

            SceneManager.LoadScene(sceneToLoad);
        }

        //This activates 'Ancient Vision' to tell the player how the current enemy compares to them on a challenge level
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(ShowAncientVision());
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            rparticle.SetActive(false);
            gparticle.SetActive(false);
            bparticle.SetActive(false);
        }
    }

    IEnumerator ShowAncientVision()
    {
        //Waiting .1 seconds in order to allow for calculation from PlayerMovement.cs -> GameManager.cs to get player level
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < enemiesInThisList.Count; i++)
        {
            totalLevel += enemiesInThisList[i].GetComponent<Unit>().CurrentLevel;
        }

        avgEnemyLevelPerEnemyInParty = totalLevel / enemiesInThisList.Count;

        if (avgEnemyLevelPerEnemyInParty >= (GameManager.avgPlayerLevelPerPlayerInParty * 3))
            rparticle.SetActive(true);
        else if (avgEnemyLevelPerEnemyInParty >= (GameManager.avgPlayerLevelPerPlayerInParty * 1.5f) && avgEnemyLevelPerEnemyInParty <= (GameManager.avgPlayerLevelPerPlayerInParty * 3))
            gparticle.SetActive(true);
        else
            bparticle.SetActive(true);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInArea = false;
        }
    }
}
