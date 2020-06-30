namespace AI
{
    public interface IBehaviourTrigger
    {
        IBehaviour TargetTrigger { get; }
        bool CheckConditions();
    }
}