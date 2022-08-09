using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AttackCover : AIstate
{
    public StateType GetState()
    {
        return StateType.AttackCover;
    }
    public void Enter(AIController agent)
    {
        agent.StartCoroutine(agent.Hide(agent.target));
    }
    public void Update(AIController agent)
    {
        if(agent.CurrentWeapon != null && agent.canSee)
        {
            agent.ReadyAttack();
            agent.CurrentWeapon.canFire = true;
            agent.CurrentWeapon.AiFire();

            if (agent.CurrentWeapon.GunStats.CurrentDurability <= 0)
                agent.BrokeWeapon(agent.WeaponStats);

        }
        if (agent.CurrentWeapon == null)
        {
            agent.stateMachine.ChangesState(StateType.Hide);
        }
        if (!agent.canSee)
        {
            agent.LostTarget();
            agent.stateMachine.ChangesState(StateType.Attack);
        }



    }
    public void Exit(AIController agent)
    {
        agent.StopCoroutine(agent.Hide(agent.target));
    }





}
