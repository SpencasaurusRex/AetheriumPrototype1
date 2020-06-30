namespace AI
{
    public class DefaultBehaviourTrigger : IBehaviourTrigger
    {
        IBehaviour target = new DefaultBehaviour();

        public IBehaviour TargetTrigger => target;
        public bool CheckConditions()
        {
            return true;            
        }
    }
}