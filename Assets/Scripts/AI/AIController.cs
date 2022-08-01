using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AIController : MonoBehaviour
{
    public NavMeshAgent Agent;
    public EnemyStats enemyStats;
    public bool ondie;
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

    [Header("Equip")]

    public Transform WeaponSocket;
    public ItemStats AiWeapon;
    public WeaponController CurrentWeapon;

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

    public AIStateMachine stateMachine;
    public Vector3 Targetposition;
    public Vector3 SeeTargetFirtTime;

    public bool walkPointSet;
    public Vector3 walkPoint;

    HitAble health;
    WeaponIK weaponIK;
    NavmeshGenerator navmeshGenerator;

    private void Start()
    {
        AiGetWeaponStarter();
        CurrentWeapon = GetComponentInChildren<WeaponController>();
        CurrentWeapon.AiEquip = true;

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AI_Attack());
        stateMachine.RegisterState(new AI_AttackCover());
        stateMachine.RegisterState(new AI_Hide());
        stateMachine.RegisterState(new AI_SearchTarget());
        stateMachine.RegisterState(new AI_Loot());
        stateMachine.RegisterState(new AI_Die());

        stateMachine.ChangesState(ChangeState);

        target = PlayerMovement.Instance.transform;

        health = GetComponent<HitAble>();
        Agent = GetComponent<NavMeshAgent>();
        weaponIK = GetComponent<WeaponIK>();
        navmeshGenerator = GetComponent<NavmeshGenerator>();
        health.OnDie += OnDie;
        health.OnDamaged += OnDamaged;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        IndexState = stateMachine.currentState;
        stateMachine.Update();

        if (target != null)
        {
            //stateMachine.ChangesState(StateType.Chase);
            if (canSee == true)
            {
                LookAtTarget();
                TargetPosition();
                //stateMachine.ChangesState(StateType.Shoot);
            }
            else
            {
                //stateMachine.ChangesState(StateType.Hide);
            }
        }


    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
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
        }
    }

    public void seeTargetFirtTime()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        //Neu qua 30, Thi di ve huong truoc mat
        Vector3 directionToTarget = transform.position + transform.forward * 20;
        if (distanceToTarget > 20)
        {
            SeeTargetFirtTime = directionToTarget;
        }
        else
        {
            SeeTargetFirtTime = target.transform.position;
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
        if (this.target == null)
        {
            return;
        }
        Vector3 normalized = (this.target.position - base.transform.position).normalized;
        Quaternion b = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, b, Time.deltaTime * 5);
    }
    public void AiGetWeaponStarter()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();
        ItemManager.Instance.GetWeaponOriginal(RandomWeapon.id, base.transform.position, base.transform.rotation, WeaponSocket);
        AiWeapon = RandomWeapon;
    }

    public void AiEquip(WeaponController weapon, ItemStats item)
    {
        WeaponController Weapon = Instantiate(weapon, WeaponSocket);
        Weapon.GunStats = item;
        Weapon.GetComponent<PickupWeapon>().item = item;

        CurrentWeapon = Weapon;

        Weapon.transform.localPosition = Vector3.zero;
        Weapon.transform.localRotation = Quaternion.identity;

        Weapon.coll.enabled = false;
        Weapon.rb.isKinematic = true;
        Weapon.AiEquip = true;

        AiWeapon = Weapon.GunStats;

        weaponIK.SetAimTransform(WeaponSocket);
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

    public void Reward()
    {

        int Moneys = Random.Range(enemyStats.minMoneyReward, enemyStats.maxMoneyReward);

        InventoryAble.Instance.RewardMoney(Moneys);
    }

    void OnDamaged(float damage)
    {
        onDamaged?.Invoke();
    }

    void OnDie()
    {
        navmeshGenerator.DestroyNavMeshData();
        stateMachine.ChangesState(StateType.Die);
        Reward();
        if (AiWeapon != null)
        {
            WeaponController Instance = Instantiate(AiWeapon.prefab, transform.position, Quaternion.identity).GetComponent<WeaponController>();
            Instance.GunStats = AiWeapon;
            Instance.GetComponent<PickupWeapon>().item = AiWeapon;
        }
        GameObject npcDead = Instantiate(enemyStats.DeadPrefab, transform.position, transform.rotation);
        npcDead.GetComponent<Rigidbody>().velocity = (-(target.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
        Destroy(gameObject);


    }
}
