using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Die : AIstate
{
    public StateType GetState()
    {
        return StateType.Die;
    }
    public void Enter(AIController agent)
    {
        Debug.Log("AiDie");
        agent.DropWeapon(agent.WeaponStats);
        agent.Reward();
    }
    public void Update(AIController agent)
    {

    }
    public void Exit(AIController agent)
    {

    }





}
