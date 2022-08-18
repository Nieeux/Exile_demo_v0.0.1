using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AIController : MonoBehaviour
{
    public enum EnemyRank
    {
        Easy,
        Normal,
        Hard,
    }
    [Header("Main")]
    public EnemyRank enemyRank;
    public NavMeshAgent Agent;
    public EnemyStats EnemyStats;
    [Header("States")]
    public StateType IndexState;
    public StateType ChangeState;

    [Header("View")]
    public float radius;

    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    public Transform target;
    public LayerMask GroundMask;
    public LayerMask targetMask;
    public LayerMask BlockTargetMask;
    public LayerMask CanHideMask;

    public bool canSee;

    

    [Header("Hide")]
    public float radiusHide;
    [Range(-1, 1)]
    [Tooltip("Lower is a better hiding spot")]
    public float HideSensitivity = 0;
    [Range(1, 10)]
    public float MinPlayerDistance = 5f;
    [Range(0, 5f)]
    public float MinObstacleHeight = 1.25f;
    [Range(0.01f, 1f)]
    public float UpdateFrequency = 0.25f;

    //public Coroutine MovementCoroutine;
    private Collider[] HidePlace = new Collider[10];
    private Collider[] rangeChecks = new Collider[2];

    public UnityAction onDamaged;
    public float healthLost;
    public bool onDamage;
    public bool remenberTarget;

    public AIStateMachine stateMachine;
    public Vector3 Targetposition;
    public Vector3 SeeTargetFirtTime;

    public bool walkPointSet;
    public Vector3 walkPoint;

    public float MaxHealth = 100;
    public float DamageMultiplier = 1f;
    public float damageFinal;
    public int damageFinalEffect;
    public GameObject numberFx;

    bool IsDead;

    public float CurrentHealth { get; set; }
    public bool Invincible { get; set; }

    WeaponIK weaponIK;
    NavmeshGenerator navmeshGenerator;
    public AiInventory inventory;

    private void Awake()
    {
        EnemyStats enemyStats = ScriptableObject.CreateInstance<EnemyStats>();
        enemyStats.Getstats(EnemyStats);
        EnemyStats = enemyStats;
        MaxHealth = EnemyStats.health;
        CurrentHealth = MaxHealth;

    }

    private void Start()
    {

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AI_Attack());
        stateMachine.RegisterState(new AI_AttackCover());
        stateMachine.RegisterState(new AI_Hide());
        stateMachine.RegisterState(new AI_SearchTarget());
        stateMachine.RegisterState(new AI_Loot());
        stateMachine.RegisterState(new AI_Die());

        stateMachine.ChangesState(ChangeState);

        PlayerStats.Instance.OnDie += PlayerOnDie;

        Agent = GetComponent<NavMeshAgent>();
        navmeshGenerator = GetComponent<NavmeshGenerator>();
        inventory = GetComponent<AiInventory>();
        weaponIK = GetComponent<WeaponIK>();

        playerRef = GameObject.FindGameObjectWithTag("Player");
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        if (enemyRank == EnemyRank.Easy)
        {
            inventory.StarterWeaponOriginal();
        }
        if (enemyRank == EnemyRank.Normal)
        {
            inventory.StarterWeaponUpgrade();
        }
        if (enemyRank == EnemyRank.Hard)
        {
            inventory.StarterWeaponAdvanced();
        }

        StartCoroutine(FOVRoutine());

        if(PlayerMovement.Instance.transform == null)
        {
            return;
        }
        if (PlayerMovement.Instance.transform != null)
        {
            target = PlayerMovement.Instance.transform;
        }
    }

    private void Update()
    {
        IndexState = stateMachine.currentState;
        stateMachine.Update();

        if (target != null)
        {
            
            if (canSee == true)
            {
                
                TargetPosition();
                
            }
            else
            {
                
            }
        }
        if (healthLost > 0f)
        {
            onDamage = true;
            base.Invoke("forgetonDamage", 1f);
        }
        else
        {
            onDamage = false;
        }

    }
    private void forgetonDamage()
    {
        healthLost = 0f;
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return wait;
            RespawnCheck();
            FieldOfViewCheck();

            if (transform.position.x == Targetposition.x && transform.position.z == Targetposition.z)
            {
                remenberTarget = false;
            }
        }
    }

    public void TargetPosition()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        //Neu qua 30, Thi di ve huong truoc mat
        Vector3 directionToTarget = transform.position + transform.forward * 20;
        if (distanceToTarget > 20)
        {
            Targetposition = directionToTarget;
        }
        else
        {
            Targetposition = target.transform.position;
            remenberTarget = true;

        }
    }

    private void FieldOfViewCheck()
    {
        rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, BlockTargetMask))
                    canSee = true;
                else
                    canSee = false;
            }
            else
                canSee = false;
        }
        else if (canSee)
            canSee = false;
    }

    public void LookAtTarget()
    {
        //if (this.target == null)
        //{
            //return;
        //}
        Vector3 normalized = (this.target.position - base.transform.position).normalized;
        Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, b, Time.deltaTime * 5);
    }

    public void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-radiusHide, radiusHide);
        float randomX = Random.Range(-radiusHide, radiusHide);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, GroundMask))
            walkPointSet = true;
    }

    public void ReadyAttack()
    {
        weaponIK.SetTargetTransform(target);
    }
    public void LostTarget()
    {
        weaponIK.SetTargetTransform(null);
    }


    public IEnumerator Hide(Transform Target)
    {

        WaitForSeconds Wait = new WaitForSeconds(UpdateFrequency);
        while (true)
        {
            for (int i = 0; i < HidePlace.Length; i++)
            {
                HidePlace[i] = null;
            }
            int hits = Physics.OverlapSphereNonAlloc(Agent.transform.position, radiusHide, HidePlace, CanHideMask);

            int hitReduction = 0;
            for (int i = 0; i < hits; i++)
            {
                if (Vector3.Distance(HidePlace[i].transform.position, Target.position) < MinPlayerDistance || HidePlace[i].bounds.size.y < MinObstacleHeight)
                {
                    HidePlace[i] = null;
                    hitReduction++;
                }
            }
            hits -= hitReduction;

            System.Array.Sort(HidePlace, ColliderArraySortComparer);

            for (int i = 0; i < hits; i++)
            {
                if (NavMesh.SamplePosition(HidePlace[i].transform.position, out NavMeshHit hit, 2f, Agent.areaMask))
                {
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, Agent.areaMask))
                    {
                        Debug.LogError($"Unable to find edge close to {hit.position}");
                    }

                    if (Vector3.Dot(hit.normal, (Target.position - hit.position).normalized) < HideSensitivity)
                    {
                        if (stateMachine.currentState == StateType.Hide || stateMachine.currentState == StateType.AttackCover)
                        {
                            Agent.SetDestination(hit.position);
                        }
                        break;
                    }
                    else
                    {
                        // Since the previous spot wasn't facing "away" enough from teh target, we'll try on the other side of the object
                        if (NavMesh.SamplePosition(HidePlace[i].transform.position - (Target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, Agent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, Agent.areaMask))
                            {
                                Debug.LogError($"Unable to find edge close to {hit2.position} (second attempt)");
                            }

                            if (Vector3.Dot(hit2.normal, (Target.position - hit2.position).normalized) < HideSensitivity)
                            {
                                if (stateMachine.currentState == StateType.Hide || stateMachine.currentState == StateType.AttackCover)
                                {
                                    Agent.SetDestination(hit2.position);
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Unable to find NavMesh near object {HidePlace[i].name} at {HidePlace[i].transform.position}");
                }
            }
            yield return Wait;
        }
    }

    public int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(Agent.transform.position, A.transform.position).CompareTo(Vector3.Distance(Agent.transform.position, B.transform.position));
        }
    }

    // Respawm
    private void RespawnCheck()
    {
        if (!IsGrounded())
        {
            SpawnEnemy.Instance.enemyList.Remove(this);
            navmeshGenerator.DestroyNavMeshData();
            Destroy(gameObject);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 10f, GroundMask);
    }


    public void Reward()
    {

        int Moneys = Random.Range(EnemyStats.minMoneyReward, EnemyStats.maxMoneyReward);

        Inventory.Instance.RewardMoney(Moneys);
    }

    // BrokeWeapon




    public void Heal(float healAmount)
    {
        float healthBefore = CurrentHealth;
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        // call OnHeal action
        float trueHealAmount = CurrentHealth - healthBefore;

    }

    public void Damage(float damage,Bullet bulletType, bool crit, Vector3 pos)
    {
        if (Invincible)
            return;

        float healthBefore = CurrentHealth;
        
        if (inventory.currentArmor != null)
        {
            DamageCalculations.ArmorResult damageMultiplier = DamageCalculations.Instance.GetDamageArmor(inventory.currentArmor, bulletType, crit, damage);
            damageFinal = damageMultiplier.damageResult;
            damageFinalEffect = (int)damageMultiplier.hitType;
        }
        else
        {
            damageFinal = damage;
        }

        CurrentHealth -= damageFinal;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        Vector3 normalized = (PlayerMovement.Instance.playerCamera.position + Vector3.up * 1.5f - pos).normalized;


        // call OnDamage action
        float trueDamageAmount = healthBefore - CurrentHealth;

        if (Vector3.Distance(PlayerMovement.Instance.playerCamera.position, base.transform.position) < 100f)
        {
            Object.Instantiate<GameObject>(this.numberFx, pos, Quaternion.identity).GetComponent<HitEffect>().SetTextAndDir((int)trueDamageAmount, normalized, (HitEffect.HitType)damageFinalEffect);
        }

        if (trueDamageAmount > 0f)
        {
            OnDamaged(trueDamageAmount);
        }
        HandleDeath();
    }

    /*
    public void InflictDamage(float damage, bool isExplosionDamage, int hitEffect, Vector3 pos)
    {
        var totalDamage = damage;

        // skip the crit multiplier if it's from an explosion
        if (!isExplosionDamage)
        {
            totalDamage *= DamageMultiplier;
        }

        // apply the damages
        Damage(totalDamage, hitEffect, pos);
    }
    */

    void HandleDeath()
    {
        if (IsDead)
            return;

        if (CurrentHealth <= 0f)
        {
            IsDead = true;
            OnDie();
        }
    }
    void OnDamaged(float damage)
    {
        healthLost = damage;
    }

    void PlayerOnDie()
    {
        stateMachine.ChangesState(StateType.idle);
    }

    void OnDie()
    {
        SpawnEnemy.Instance.enemyList.Remove(this);

        navmeshGenerator.DestroyNavMeshData();
        stateMachine.ChangesState(StateType.Die);

        GameObject npcDead = Instantiate(EnemyStats.DeadPrefab, transform.position, transform.rotation);
        npcDead.GetComponent<Rigidbody>().velocity = (-(target.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
        Destroy(gameObject);

    }
}
