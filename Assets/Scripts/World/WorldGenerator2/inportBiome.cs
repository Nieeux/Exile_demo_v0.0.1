using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inportBiome : MonoBehaviour
{
    public WorldManager terrainManager;
    public TerrainStats biome;
    public GameObject player;
    private void Start()
    {
        this.terrainManager.Init(player.transform.position, biome);
    }

    // Update is called once per frame
    void Update()
    {

    }
}