using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Idle : MonoBehaviour, AIstate
{
    public void AiEnter(AIController agent)
    {

    }

    public void AiExit(AIController agent)
    {

    }

    public void AiUpdate(AIController agent)
    {
        agent.ReadyAttack();
    }

    public StateType GetState()
    {
        return StateType.idle;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
