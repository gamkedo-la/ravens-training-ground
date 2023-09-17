using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public Unit CurrentUnit    
    {
        get { return currentUnit; }
        set
        {
            Targets = null;
            currentUnit = value;
        }
    }
    public Unit currentUnit;


    public List<Unit> targets = new List<Unit>();
    public List<Unit> Targets
    {
        get { return targets; }
        set
        {
            //Turn off all last targets visual
            SetTargetsVisualState(false);
            targets = value;
            //Turn on all new targets visual
            SetTargetsVisualState(true);
        }
    }

    public void SetCurrentUnit(Unit currentUnit)
    { this.CurrentUnit = currentUnit; }

    public void SetTargetsVisualState(bool targeted)
    {
        if (targets != null)
            foreach (Unit target in targets)
            {
                target.targetParticle.SetActive(targeted);
            }
    }
}
