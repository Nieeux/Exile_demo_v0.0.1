using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Hide : AIstate
{


    StateType AIstate.GetState()
    {
        return StateType.Hide;
    }
    void AIstate.AiEnter(AIController agent)
    {
        agent.StartCoroutine(agent.Hide(agent.target));
    }
    void AIstate.AiUpdate(AIController agent)
    {

    }


    void AIstate.AiExit(AIController agent)
    {
        agent.StopCoroutine(agent.Hide(agent.target));
        //agent.HidePlace = null;
    }
}
