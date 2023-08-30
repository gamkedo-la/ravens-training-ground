using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIControllor : MonoBehaviour
{
    [SerializeField]
    BattlePlayerProfileController battlePlayerProfileControllerPrefab;
    public List<BattlePlayerProfileController> playerProfileControllers;
    
    public void Initialize(List<Unit> units)
    {
        int index = 0;
        foreach(Unit unit in units)
        {
            BattlePlayerProfileController tempPlayerProfileController = Instantiate(battlePlayerProfileControllerPrefab,this.transform);
            tempPlayerProfileController.Intitialize(unit,index);
            index++;
        }
    }
}
