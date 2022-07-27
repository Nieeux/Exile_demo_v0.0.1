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
        //GameObject npcDead = Instantiate(agent.npcDeadPrefab, agent.transform.position, agent.transform.rotation);
       // npcDead.GetComponent<Rigidbody>().velocity = (-(agent.target.position - agent.transform.position).normalized * 8) + new Vector3(0, 5, 0);

        //Destroy(gameObject);
    }
    public void Update(AIController agent)
    {

    }
    public void Exit(AIController agent)
    {

    }





}
