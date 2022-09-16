using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SearchTarget : MonoBehaviour, AIstate
{

    public StateType GetState()
    {
        return StateType.SearchTarget;
    }
    public void AiEnter(AIController agent)
    {
        agent.StartCoroutine(agent.Search());

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
                agent.stateMachine.ChangesState(StateType.AttackCover);
                //agent.healthLost = 0;
            }
            
            else if (agent.remenberTarget == false)
            {
                
            }
            


        }

    }

    public void AiExit(AIController agent)
    {
        agent.StopCoroutine(agent.Search());
    }
}
