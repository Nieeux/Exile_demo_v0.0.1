using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour
{
    public float attackDistance = 3f;
    public float movementSpeed = 4f;
    public float EnemyHP;
    public float enemyDamage;
    public float attackRate = 0.5f;

    public Transform firePoint;
    public GameObject npcDeadPrefab;
    public ActiveEvent enemyManager;
    public ItemDrop randomItemDrop;
    HitAble health;

    [Header("Loot")]
    public GameObject LootPrefab;
    [Range(0, 1)]
    public float DropRate = 1f;

    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public ActiveEvent es;
    NavMeshAgent agent;
    float nextAttackTime = 0;
    public float Timer;

    public UnityAction onDamaged;

    void Start()
    {
        health = GetComponent<HitAble>();
        health.OnDie += OnDie;
        health.OnDamaged += OnDamaged;

        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        agent.speed = movementSpeed;

        //Set Rigidbody to Kinematic to prevent hit register bug
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    void Awake()
    {
        // Địch khỏe hơn sau 30 kẻ địch 
        enemyManager = FindObjectOfType<ActiveEvent>();
        EnemyHP = 100 + (2 * enemyManager.waveNumber);
        enemyDamage = 5 + (2 * enemyManager.waveNumber);

    }

    void Update()
    {
        //Tấn công Player
        if (agent.remainingDistance - attackDistance < 0.01f)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;
                RaycastHit hit;
                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);

                        PlayerStats player = hit.transform.GetComponent<PlayerStats>();
                        player.Damage(enemyDamage);
                    }
                }
            }
        }
        //Move towardst he player
        agent.destination = playerTransform.position;
        //Always look at player
        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));
    }
    //Địch nhận DMG
    void OnDamaged(float damage)
    {
        onDamaged?.Invoke();
    }
    void OnDie()
    {
        ItemDrop randomitem = Instantiate(randomItemDrop, transform.position, transform.rotation);
        GameObject npcDead = Instantiate(npcDeadPrefab, transform.position, transform.rotation);
        npcDead.GetComponent<Rigidbody>().velocity = (-(playerTransform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);

        Destroy(gameObject);
    }

    //Địch nhận DMG và chết
    /*
    public void ApplyDamage(float points)
    {
        EnemyHP -= points;
        if (EnemyHP <= 0)
        {
            //Spawn xác và item
            ItemDrop randomitem = Instantiate(randomItemDrop, transform.position, transform.rotation);
            GameObject npcDead = Instantiate(npcDeadPrefab, transform.position, transform.rotation);

            //Xác kẻ địch bật ra đằng sau
            npcDead.GetComponent<Rigidbody>().velocity = (-(playerTransform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
            //Destroy(npcDead, 1);
            //es.EnemyEliminated(this);
            Destroy(gameObject);
        }
    }
    */

    public bool TryDropItem()
    {
        if (DropRate == 0 || LootPrefab == null)
            return false;
        else if (DropRate == 1)
            return true;
        else
            return (UnityEngine.Random.value <= DropRate);
    }

}
