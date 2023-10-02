using Player;
using UnityEngine;

namespace FSM
{
    // Hierarchical State machine
    // I looked into Pushdown automata but this isnt really neceserry for this system
    // https://github.com/TS696/PdStateMachine/tree/master << only real recourse i found (in unity)
    // For maybe a complexer system that needs state history it might be a better fit

    public class StateController
    {
        public BaseState currentState;

        // State Definitions
        public GroundedState GroundedState = new();
        public WalkState WalkState = new();
        public RunState RunState = new ();
        public CrouchState CrouchState = new();
        public JumpState JumpState = new();
        
        public InAirState InAirState = new();
        public InAIrJumpState InAIrJumpState = new();
        public LedgeGrabState LedgeGrabState = new();
        
        
        private CharacterMovementComponent cmc;

        public StateController(CharacterMovementComponent _cmc)
        {
            cmc = _cmc;
            ChangeState(GroundedState);
            GameManager.GlobalUpdate += Update;
            GameManager.GlobalFixedUpdate += FixedUpdate;
        }
        
        private void Update()
        {
            Debug.Log(currentState);
            currentState?.OnStateUpdate();
        }

        private void FixedUpdate()
        {
            //player.PlayerMove();
            currentState?.OnStateFixedUpdate();
        }

        public void ChangeState(BaseState _newState)
        {
            currentState?.OnStateExit();

            currentState = _newState;
            currentState.OnStateEnter(this, cmc);
        }
    }
}