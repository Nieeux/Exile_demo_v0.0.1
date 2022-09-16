using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Attack : MonoBehaviour, AIstate
{
    public StateType GetState()
    {
        return StateType.Attack;
    }

    public void AiEnter(AIController agent)
    {
        //agent.Agent.destination = agent.Targetposition;
    }

    public void AiUpdate(AIController agent)
    {
        
        if (agent.canSee == true)
        {
            agent.LookAtTarget();
            agent.Agent.destination = agent.Targetposition;
            agent.stateMachine.ChangesState(StateType.AttackCover);
        }
        else
        {
            agent.stateMachine.ChangesState(StateType.SearchTarget);
        }

    }
    public void AiExit(AIController agent)
    {

    }


}
