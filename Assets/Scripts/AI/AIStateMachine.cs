
public class AIStateMachine
{
    public AIstate[] states;
    public AIController agent;
    public StateType currentState;

    public AIStateMachine(AIController agent)
    {
        this.agent = agent;
        int numstates = System.Enum.GetNames(typeof(StateType)).Length;
        states = new AIstate[numstates];
    }

    public void RegisterState(AIstate state)
    {
        int index = (int)state.GetState();
        states[index] = state;
    }

    public AIstate GetState(StateType statetype)
    {
        int index = (int)statetype;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.AiUpdate(agent);
    }

    public void ChangesState(StateType newstate)
    {
        GetState(currentState)?.AiExit(agent);
        currentState = newstate;
        GetState(currentState)?.AiEnter(agent);
    }

}
