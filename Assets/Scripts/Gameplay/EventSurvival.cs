using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EventSurvival : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Player player;

    public float EnemyInterval = 1f;
    public bool WaveEvent = false;

    public int EnemiesSpawned = 0; // So Enemy da spawn
    public int waveNumber = 0;

    void Start()
    {
        Debug.Log("Survival");
        StartCoroutine(SpawnEnemy(EnemyInterval, enemyPrefab));
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        // Sau khi spawn ra 30 kẻ địch waveNumber++
        if (EnemiesSpawned >= 30)
        {
            waveNumber++;
            EnemiesSpawned = 0;
        }
    }

    public void Trigger()
    {

        //StopAllCoroutines();
    }

    //Spawm Enemy
    private IEnumerator SpawnEnemy(float interval, GameObject enemyPrefab)
    {
        // Đếm thời gian
        yield return new WaitForSeconds(interval);
        StartCoroutine(SpawnEnemy(EnemyInterval, enemyPrefab));

        Vector3 position = base.transform.position;
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);

        EnemyController npc = enemy.GetComponent<EnemyController>();
        npc.playerTransform = player.transform;
        //npc.es = this;
        EnemiesSpawned++;
    }

}