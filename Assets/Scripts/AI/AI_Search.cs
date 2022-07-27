using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Search : AIstate
{
    public float padleft = 0;
    public StateType GetState()
    {
        return StateType.Search;
    }
    public void Enter(AIController agent)
    {
        //float Rot = agent.transform.localRotation.eulerAngles.y;

    }
    public void Update(AIController agent)
    {
        if (agent.canSee == true)
        {
            agent.stateMachine.ChangesState(StateType.Attack);
        }
        else
        {
            //float Rot = agent.transform.localRotation.eulerAngles.y;
            //this.padleft = (int)Mathf.Lerp(this.padleft, Rot / 2, Time.deltaTime * 100f);

            //agent.transform.eulerAngles = new Vector3(agent.transform.eulerAngles.x, padleft, agent.transform.eulerAngles.z);
        }
    }
    public void Exit(AIController agent)
    {

    }

}
