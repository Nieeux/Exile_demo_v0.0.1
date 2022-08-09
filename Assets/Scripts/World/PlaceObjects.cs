using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    /*
    public WorldGenerator TerrainController;
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
        //this.randomGen = new Randomly(NoiseSettings.seed);
        int numObjects = 1;
        for (int i = 0; i < numObjects; i++)
        {
            //float x = (float)(this.randomGen.NextDouble() * 2.0 - 1.0) * 240 / 2f;
            //float z = (float)(this.randomGen.NextDouble() * 2.0 - 1.0) * 240 / 2f;
            //Vector3 startPoint = new Vector3(x, 0f, z);
            Vector3 startPoint = RandomPointAboveTerrain();
            startPoint.y = 10f;
            int prefabType = Random.Range(0, TerrainController.PlaceableObjects.Length);

            // Raycast neu gap Ground thi spawn
            RaycastHit hit;
            if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.point.y > TerrainController.transform.position.y && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, TerrainController.PlaceableObjectSizes[prefabType], Vector3.down, out boxHit, orientation) && boxHit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    Instantiate(TerrainController.PlaceableObjects[prefabType], new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, transform);
                        //tao doi tuong va truy cap RandomEvents
                    //TriggerArea spawnZone = Instantiate(TerrainController.PlaceableObjects[prefabType], new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, transform).GetComponent<TriggerArea>();
                        //Set id cho RandomEvents
                    //spawnZone.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());


                }
                //Debug code. To use, uncomment the giant thingy below
                //Debug.DrawRay(startPoint, Vector3.down * 10000, Color.blue);
                //DrawBoxCastBox(startPoint, TerrainController.PlaceableObjectSizes[prefabType], orientation, Vector3.down, 10000, Color.red);
                //UnityEditor.EditorApplication.isPaused = true;
            }

        }
    }


    private Vector3 RandomPointAboveTerrain()
    {
        return new Vector3(
            // random diem tao objects
            Random.Range(transform.position.x - TerrainController.TerrainSize.x / 2, transform.position.x + TerrainController.TerrainSize.x / 2),transform.position.y + TerrainController.TerrainSize.y * 2,
            Random.Range(transform.position.z - TerrainController.TerrainSize.z / 2, transform.position.z + TerrainController.TerrainSize.z / 2));
    }
    */
}
