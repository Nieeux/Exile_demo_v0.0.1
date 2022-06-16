using UnityEngine;

public class RandomEvents : MonoBehaviour, SharedObject
{
    public itemToSpawn[] itemToSpawn;
    public int id;

    private void Start()
    {
        EventManager.current.EnterRandomevents += Randomevents;
    }

    // Start is called before the first frame update
    public void Randomevents(int id)
    {
        Debug.Log("Da RandomEvents");
        if (id == this.id)
        {
            for (int i = 0; i < itemToSpawn.Length; i++)
            {
                if (i == 0)
                {
                    itemToSpawn[i].minSpawnProb = 0;
                    itemToSpawn[i].maxSpawnProb = itemToSpawn[i].spawnRate - 1;
                }
                else
                {
                    itemToSpawn[i].minSpawnProb = itemToSpawn[i - 1].maxSpawnProb + 1;
                    itemToSpawn[i].maxSpawnProb = itemToSpawn[i].minSpawnProb + itemToSpawn[i].spawnRate - 1;
                }
            }
            RandomEvent();

            void RandomEvent()
            {
                float random = Random.Range(0, 100);

                for (int i = 0; i < itemToSpawn.Length; i++)
                {
                    if (random >= itemToSpawn[i].minSpawnProb && random <= itemToSpawn[i].maxSpawnProb)
                    {
                        Instantiate(itemToSpawn[i].item, transform.position, Quaternion.identity);
                        break;
                    }
                }
            }
        }
    }
    public void SetId(int id)
    {
        this.id = id;
    }

    public int GetId()
    {
        return this.id;
    }
}
[System.Serializable]
public class EventToSpawn
{
    public GameObject item;
    public float spawnRate;
    [HideInInspector] public float minSpawnProb, maxSpawnProb;
}
