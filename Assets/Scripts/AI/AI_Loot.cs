using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Loot : AIstate
{
    public StateType GetState()
    {
        return StateType.Loot;
    }
    public void AiEnter(AIController agent)
    {
        PickupWeapon pickup = FindItem(agent);
        agent.Agent.destination = pickup.transform.position;

    }
    public void AiUpdate(AIController agent)
    {
        if(agent.inventory.CurrentWeapon != null)
        {
            agent.stateMachine.ChangesState(StateType.Attack);
        }
        else
        {
        }

    }

    public void AiExit(AIController agent)
    {

    }

    public PickupWeapon FindItem(AIController agent)
    {
        PickupWeapon Item = Object.FindObjectOfType<PickupWeapon>();
        PickupWeapon ClosestItem = null;
        float ClosestDistance = float.MaxValue;
        float DistanceToItem = Vector3.Distance(agent.transform.position, Item.transform.position);
        if (DistanceToItem < ClosestDistance)
        {
            ClosestDistance = DistanceToItem;
            ClosestItem = Item;
        }
        return ClosestItem;
    }




}
