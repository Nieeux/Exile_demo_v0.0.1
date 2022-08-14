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

    void AiEnter(AIController agent);

    void AiUpdate(AIController agent);

    void AiExit(AIController agent);

}
