using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_AttackCover : MonoBehaviour, AIstate
{
    public StateType GetState()
    {
        return StateType.AttackCover;
    }
    public void AiEnter(AIController agent)
    {
        agent.StartCoroutine(agent.Hide(agent.target));
    }
    public void AiUpdate(AIController agent)
    {
        if(agent.inventory.CurrentWeapon != null && agent.canSee)
        {
            agent.LookAtTarget();
            agent.ReadyAttack();
            agent.inventory.CurrentWeapon.AiFire();

            if (agent.inventory.CurrentWeapon.GunStats.CurrentDurability <= 0)
                agent.inventory.BrokeWeapon(agent.inventory.WeaponStats);

        }
        if (agent.inventory.CurrentWeapon == null)
        {
            agent.stateMachine.ChangesState(StateType.Hide);
        }
        if (!agent.canSee)
        {
            agent.LostTarget();
            agent.stateMachine.ChangesState(StateType.Attack);
        }



    }
    public void AiExit(AIController agent)
    {
        agent.StopCoroutine(agent.Hide(agent.target));
    }





}
