using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SearchTarget : MonoBehaviour, AIstate
{
    public float padleft = 0;


    public StateType GetState()
    {
        return StateType.SearchTarget;
    }
    public void AiEnter(AIController agent)
    {
        //float Rot = agent.transform.localRotation.eulerAngles.y;

    }
    public void AiUpdate(AIController agent)
    {


        if (agent.canSee == true)
        {
            agent.stateMachine.ChangesState(StateType.Attack);
        }
        else
        {

            if (agent.onDamage == true)
            {
                agent.LookAtTarget();
                if (agent.canSee != true)
                {
                    agent.stateMachine.ChangesState(StateType.AttackCover);
                }
                //agent.healthLost = 0;
            }
            
            else if (agent.remenberTarget == false)
            {

                if (!agent.walkPointSet) agent.Invoke("SearchWalkPoint", 5f);
                if (agent.walkPointSet)
                    agent.Agent.SetDestination(agent.walkPoint);

                Vector3 distanceToWalkPoint = agent.transform.position - agent.walkPoint;

                //Walkpoint reached
                if (distanceToWalkPoint.magnitude < 1f)
                    agent.walkPointSet = false;
            }
            


        }
    }
    public void AiExit(AIController agent)
    {

    }
}
