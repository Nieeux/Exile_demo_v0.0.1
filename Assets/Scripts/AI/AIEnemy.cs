using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    public Transform target;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }
    private void Update()
    {
        if (canSeePlayer == true)
        {
            agent.destination = target.transform.position;
            LookAtTarget();
        }
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
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
}
