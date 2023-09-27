using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshGenerator : MonoBehaviour
{
    public NavMeshSurface surfaceToGenerate;
    private void Start()
    {
        Invoke("TriggerBuild", 1);
    }

    public void TriggerBuild()
    {
        surfaceToGenerate.BuildNavMesh();
    }
}
