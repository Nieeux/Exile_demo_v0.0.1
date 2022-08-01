using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Attack : AIstate
{
    public StateType GetState()
    {
        return StateType.Attack;
    }

    public void Enter(AIController agent)
    {
        agent.Agent.destination = agent.Targetposition;
        agent.seeTargetFirtTime();
    }

    public void Update(AIController agent)
    {
        
        if (agent.canSee == true)
        {
            agent.Agent.destination = agent.Targetposition;
            agent.stateMachine.ChangesState(StateType.AttackCover);
        }
        else
        {
            agent.stateMachine.ChangesState(StateType.SearchTarget);
        }

    }
    public void Exit(AIController agent)
    {

    }


}
