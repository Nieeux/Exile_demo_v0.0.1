public enum StateType
{
    idle,
    Attack,
    AttackCover,
    SearchTarget,
    SearchLoot,
    Loot,
    Hide,
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
