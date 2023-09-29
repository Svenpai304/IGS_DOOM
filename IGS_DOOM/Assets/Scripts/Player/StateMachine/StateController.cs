using UnityEngine;

namespace FSM
{
    public class StateController
    {
        public BaseState currentState;

        // State Definitions
        public GroundedState GroundedState = new();
        public IdleState IdleState = new();
        public WalkState WalkState = new();
        public RunState RunState = new ();
        public CrouchState CrouchState = new();
        public JumpState JumpState = new();
        
        public InAirState InAirState = new();
        public InAIrJumpState InAIrJumpState = new();
        public LedgeGrabState LedgeGrabState = new();
        
        
        public Player.Player player;
        
        public StateController(Player.Player _player)
        {
            player = _player;
            ChangeState(GroundedState);
        }

        public void Update()
        {
            Debug.Log(currentState);
            currentState?.OnStateUpdate();
        }

        public void FixedUpdate()
        {
            currentState?.OnStateFixedUpdate();
        }

        public void ChangeState(BaseState _newState)
        {
            currentState?.OnStateExit();

            currentState = _newState;
            currentState.OnStateEnter(this);
        }
    }
}