using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingMonster : MonoBehaviour
{
    int enemyCount;
    public List<string> enemiesInThisFight = new List<string>();
    public List<string> enemiesFloor1 = new List<string>();

    public List<GameObject> enemiesInThisList = new List<GameObject>();

    int totalLevel;

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

}
