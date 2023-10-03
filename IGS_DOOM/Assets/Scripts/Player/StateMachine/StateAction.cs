namespace FSM
{
    public abstract class StateAction
    {
        public abstract void Execute(IStateData data);
    }
}