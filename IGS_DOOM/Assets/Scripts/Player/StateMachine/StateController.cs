namespace FSM
{
    public class StateController
    {
        private BaseState currentState;
        private GroundedState groundedState = new GroundedState();
        
        void Start()
        {
            ChangeState(groundedState);
        }

        void Update()
        {
            currentState?.OnStateUpdate();
        }

        void FixedUpdate()
        {
            currentState?.OnStateFixedUpdate();
        }

        void ChangeState(BaseState _newState)
        {
            currentState?.OnStateExit();

            currentState = _newState;
            currentState.OnStateEnter(this);
        }
    }
}