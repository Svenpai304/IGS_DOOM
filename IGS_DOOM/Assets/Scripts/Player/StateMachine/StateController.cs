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
            if (currentState != null)
            {
                currentState.OnStateUpdate();
            }
        }

        void ChangeState(BaseState _newState)
        {
            if (currentState != null)
            {
                currentState.OnStateExit();
            }

            currentState = _newState;
            currentState.OnStateEnter(this);
        }
    }
}