using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomSupply : MonoBehaviour
{
    public float totalWeight { get; set; }
    protected Random randomly;
    public RareSupply[] allSupply;

    void Start()
    {
        DropSupply();
    }

    void DropSupply()
    {
        this.randomly = new Random();
        this.CalculateRare();
        GameObject gameObject = this.FindSupplyToSpawn(this.allSupply, this.totalWeight, this.randomly);
        GameObject gameObject2 = Instantiate(gameObject, transform.position, transform.rotation, transform);
        //Destroy(this);
    }

    public void CalculateRare()
    {
        this.totalWeight = 0f;
        foreach (RareSupply weightedSpawn in this.allSupply)
        {
            this.totalWeight += weightedSpawn.rare;
        }
    }

    public GameObject FindSupplyToSpawn(RareSupply[] structurePrefabs, float totalWeight, Random rand)
    {
        float num = (float)rand.NextDouble();
        float num2 = 0f;
        for (int i = 0; i < structurePrefabs.Length; i++)
        {
            num2 += structurePrefabs[i].rare;
            if (num < num2 / totalWeight)
            {
                return structurePrefabs[i].prefab;
            }
        }
        return structurePrefabs[0].prefab;
    }

    [System.Serializable]
    public class RareSupply
    {
        public GameObject prefab;
        public float rare;
    }
}
