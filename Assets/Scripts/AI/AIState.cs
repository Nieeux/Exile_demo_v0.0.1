public enum StateType
{
    Attack,
    AttackCover,
    Search,
    Hide,
    Loot,
    Quit,
    Die,
}

public interface AIstate
{
    StateType GetState();

    void Enter(AIController agent);

    void Update(AIController agent);

    void Exit(AIController agent);

}
