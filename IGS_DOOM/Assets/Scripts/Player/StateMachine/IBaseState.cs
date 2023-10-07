namespace FSM
{
    public delegate void StateEvent(IBaseState _state);
    
    // Changed to Interface based Statemachine because Class based didnt work with the StateEvent
    // When passing a state to the event it gave a NULL Exception for some reason
    // (i tried to fix it but nothing seemed to work until I changed it to a interface)
    public interface IBaseState
    {
        //protected IStateData data;

        public void OnStateEnter(IStateData _data) {}

        public void OnStateUpdate(IStateData _data) {}

        public void OnStateFixedUpdate(IStateData _data) {}

        public void OnStateExit(IStateData _data) {}

        public StateEvent SwitchState { get; set; }
    }

    public interface IStateData
    {
        public ScratchPad SharedData { get; }
    }
}
