using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEvent : MonoBehaviour, SharedObject
{
    public GameObject enemyPrefab;
    public PlayerMovement player;

    public float EnemyInterval = 1f;

    public int EnemiesSpawned = 0; // So Enemy da spawn
    public int waveNumber = 0;

    public int id;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnEnemy(EnemyInterval, enemyPrefab));
            player = FindObjectOfType<PlayerMovement>();
            //EventManager.current.Randomevents(id);
            Debug.Log("Da trigger");
            //id++;
        }
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemyPrefab)
    {
        // ??m th?i gian
        yield return new WaitForSeconds(interval);
        StartCoroutine(SpawnEnemy(EnemyInterval, enemyPrefab));

        Vector3 position = base.transform.position + new Vector3(Random.Range(-1f, 1f) * 10f, 0f, Random.Range(-1f, 1f) * 10f);
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);

        EnemyController npc = enemy.GetComponent<EnemyController>();
        npc.playerTransform = player.transform;
        npc.es = this;
        EnemiesSpawned++;
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


