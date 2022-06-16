using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyStats : MonoBehaviour
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

    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public ActiveEvent es;
    NavMeshAgent agent;
    float nextAttackTime = 0;
    public float Timer;

    void Start()
    {
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
                        player.TakeDamage(enemyDamage);
                    }
                }
            }
        }
        //Move towardst he player
        agent.destination = playerTransform.position;
        //Always look at player
        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));
    }

    //Địch nhận DMG và chết
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

}
