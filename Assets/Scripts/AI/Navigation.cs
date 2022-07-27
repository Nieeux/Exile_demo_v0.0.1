using System.Collections;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Navigation : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface Surfaces;
    [SerializeField]
    private AIController Agent;
    [SerializeField]
    private float UpdateRate = 0.1f;
    [SerializeField]
    private float MovementThreshold = 3f;
    [SerializeField]
    private Vector3 NavMeshSize = new Vector3(20, 20, 20);
    private Vector3 WorldAnchor;

    private NavMeshData NavMeshDatas;
    private List<NavMeshBuildSource> SourcesPerSurface = new List<NavMeshBuildSource>();
    public delegate void NavMeshUpdatedEvent(Bounds Bounds);
    public NavMeshUpdatedEvent OnNavMeshUpdate;

    private void Awake()
    {
        NavMeshDatas = new NavMeshData();
        NavMesh.AddNavMeshData(NavMeshDatas);
        SourcesPerSurface.Add(new NavMeshBuildSource());
        BuildNavMesh(false);
        StartCoroutine(CheckMovement());
    }

    void Start()
    {
        
    }

    private IEnumerator CheckMovement()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        while (true)
        {
            if (Agent != null)
            {
                if (Vector3.Distance(WorldAnchor, Agent.transform.position) > MovementThreshold)
                {
                    BuildNavMesh(true);
                    WorldAnchor = Agent.transform.position;
                }
            }
            else
            {
                Surfaces.RemoveData();
                StopCoroutine(CheckMovement());
            }

            yield return Wait;
        }

    }

    void BuildNavMesh(bool Async)
    {
        Bounds navMeshBounds = new Bounds(Agent.transform.position, NavMeshSize);
        if (Async)
        {
            AsyncOperation navMeshUpdateOperation = NavMeshBuilder.UpdateNavMeshDataAsync(NavMeshDatas, Surfaces.GetBuildSettings(), SourcesPerSurface, navMeshBounds);
            navMeshUpdateOperation.completed += HandleNavMeshUpdate;
        }
        else
        {
            NavMeshBuilder.UpdateNavMeshData(NavMeshDatas, Surfaces.GetBuildSettings(), SourcesPerSurface, navMeshBounds);
            OnNavMeshUpdate?.Invoke(navMeshBounds);
        }
    }
    private void HandleNavMeshUpdate(AsyncOperation Operation)
    {
        OnNavMeshUpdate?.Invoke(new Bounds(WorldAnchor, NavMeshSize));
    }
}
