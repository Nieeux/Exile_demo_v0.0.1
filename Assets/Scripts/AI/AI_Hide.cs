using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Hide : AIstate
{


    StateType AIstate.GetState()
    {
        return StateType.Hide;
    }
    void AIstate.Enter(AIController agent)
    {
        agent.StartCoroutine(agent.Hide(agent.target));
    }
    void AIstate.Update(AIController agent)
    {
        if (agent.canSee && !agent.CurrentWeapon.reloading)
        {
            agent.stateMachine.ChangesState(StateType.Attack);
        }

    }


    void AIstate.Exit(AIController agent)
    {
        agent.StopCoroutine(agent.Hide(agent.target));
        //agent.HidePlace = null;
    }
}
