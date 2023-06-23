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

    bool isInArea;

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
            print(enemiesInThisFight[i] + enemiesInThisList[i].GetComponent<Unit>().CurrentLevel);
        }

        print(totalLevel);  
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
