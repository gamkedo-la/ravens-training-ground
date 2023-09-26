using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGenerator : MonoBehaviour
{
    private void Start()
    {
        Invoke("TriggerBuild", 2);
    }

    public void TriggerBuild()
    {
        this.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
