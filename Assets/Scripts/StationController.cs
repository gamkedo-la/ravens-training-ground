using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
    public Unit stationedUnit;
    public Transform perspectiveCamera;
    public List<StationController> neighborStations;

    public Vector3 platformOffset = new Vector3(0, 1, 0);

    public void AddUnit(Unit unit)
    {
        stationedUnit = unit;
        unit.transform.position = transform.position + platformOffset;
        unit.transform.rotation = transform.rotation;

        unit.stationController= this;
    }
    public List<Unit> GetNeighborUnits()
    {
        List<Unit> units = new List<Unit>();
        foreach(StationController stationController in neighborStations) 
        {
            if (stationedUnit != null)
                units.Add(stationController.stationedUnit);
            else
                Debug.Log($"No Unit on station {gameObject.name}");
        }
        return units;
    }

}
