using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public WorldGenerator TerrainController;
    //public Randomly randomGen;
    public bool Spawn;
    void Start()
    {
        TerrainController = FindObjectOfType<WorldGenerator>();

        if (this.Spawn)
        {
            Place();
        }
    }
    public void Place()
    {
        Vector3 startPoint = RandomPointAboveTerrain();
        //TerrainController.Ground.transform.localScale = Vector3.one * 240 / 10f;
        Instantiate(TerrainController.Ground, new Vector3(startPoint.x, startPoint.y, startPoint.z), gameObject.transform.rotation, transform);
        //GameObject meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //meshObject.transform.localScale = Vector3.one * 240 / 10f;
        //meshObject.transform.parent = transform;

    }
    private Vector3 RandomPointAboveTerrain()
    {
        return new Vector3(transform.position.x, transform.position.y , transform.position.z);
    }
}
