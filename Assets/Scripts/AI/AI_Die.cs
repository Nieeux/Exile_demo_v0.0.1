using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Die : AIstate
{
    public StateType GetState()
    {
        return StateType.Die;
    }
    public void AiEnter(AIController agent)
    {
        Debug.Log("AiDie");
        agent.inventory.DropItem();
        agent.Reward();
    }
    public void AiUpdate(AIController agent)
    {

    }
    public void AiExit(AIController agent)
    {

    }





}
