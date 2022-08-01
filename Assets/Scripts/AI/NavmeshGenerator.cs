using System.Collections;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavmeshGenerator : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface Surface;
    [SerializeField]
    private AIController Player;
    [SerializeField]
    private float UpdateRate = 0.1f;
    [SerializeField]
    private float MovementThreshold = 3f;
    [SerializeField]
    private Vector3 NavMeshSize = new Vector3(20, 20, 20);

    public delegate void NavMeshUpdatedEvent(Bounds Bounds);
    public NavMeshUpdatedEvent OnNavMeshUpdate;

    private Vector3 WorldAnchor;
    private NavMeshData NavMeshData;
    private List<NavMeshBuildSource> Sources = new List<NavMeshBuildSource>();

    private void Awake()
    {
        NavMeshData = new NavMeshData();
        NavMesh.AddNavMeshData(NavMeshData);
        BuildNavMesh(false);
        StartCoroutine(CheckPlayerMovement());
    }
    private void Start()
    {
        base.Invoke("BuildNavMeshStart", 1f);
    }
    private void BuildNavMeshStart()
    {
        BuildNavMesh(true);
    }
    public void DestroyNavMeshData()
    {
        if (NavMeshData == true)
        {
            NavMeshData.Destroy(NavMeshData);
        }
    }
    private IEnumerator CheckPlayerMovement()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        while (true)
        {
            if (Vector3.Distance(WorldAnchor, Player.transform.position) > MovementThreshold)
            {
                BuildNavMesh(true);
                WorldAnchor = Player.transform.position;
            }

            yield return Wait;
        }
    }

    private void BuildNavMesh(bool Async)
    {
        Bounds navMeshBounds = new Bounds(Player.transform.position, NavMeshSize);
        List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();

        List<NavMeshModifier> modifiers;
        if (Surface.collectObjects == CollectObjects.Children)
        {
            modifiers = new List<NavMeshModifier>(GetComponentsInChildren<NavMeshModifier>());
        }
        else
        {
            modifiers = NavMeshModifier.activeModifiers;
        }

        for (int i = 0; i < modifiers.Count; i++)
        {
            if (((Surface.layerMask & (1 << modifiers[i].gameObject.layer)) == 1)
                && modifiers[i].AffectsAgentType(Surface.agentTypeID))
            {
                markups.Add(new NavMeshBuildMarkup()
                {
                    root = modifiers[i].transform,
                    overrideArea = modifiers[i].overrideArea,
                    area = modifiers[i].area,
                    ignoreFromBuild = modifiers[i].ignoreFromBuild
                });
            }
        }

        if (Surface.collectObjects == CollectObjects.Children)
        {
            NavMeshBuilder.CollectSources(transform, Surface.layerMask, Surface.useGeometry, Surface.defaultArea, markups, Sources);
        }
        else
        {
            NavMeshBuilder.CollectSources(navMeshBounds, Surface.layerMask, Surface.useGeometry, Surface.defaultArea, markups, Sources);
        }

        //Sources.RemoveAll(source => source.component != null && source.component.gameObject.GetComponent<NavMeshAgent>() != null);

        if (Async)
        {
            AsyncOperation navMeshUpdateOperation = NavMeshBuilder.UpdateNavMeshDataAsync(NavMeshData, Surface.GetBuildSettings(), Sources, navMeshBounds);
            navMeshUpdateOperation.completed += HandleNavMeshUpdate;
        }
        else
        {
            NavMeshBuilder.UpdateNavMeshData(NavMeshData, Surface.GetBuildSettings(), Sources, navMeshBounds);
            OnNavMeshUpdate?.Invoke(navMeshBounds);
        }
    }
    private void HandleNavMeshUpdate(AsyncOperation Operation)
    {
        OnNavMeshUpdate?.Invoke(new Bounds(WorldAnchor, NavMeshSize));
    }
}