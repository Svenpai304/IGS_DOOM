using UnityEngine;

namespace FSM
{
    // State machine
    // I looked into Pushdown Automata but this isn't really necessary for this system
    // https://github.com/TS696/PdStateMachine/tree/master << only real recourse i found (in unity)
    // For maybe a complexer system that needs state history it might be a better fit
    // But DOOM 4 doesnt really have that complex of a movement system so you dont need to know the prev
    // state or anything, so that's why I just went for a normal Finite State Machine

    public class StateController
    {
        private IBaseState currentState;
        private IStateData sharedData;
        
        // Initialize global state variables
        public static WalkState WalkState = new();
        public static RunState RunState = new ();
        public static CrouchState CrouchState = new();
        public static JumpState JumpState = new();
        public static LedgeGrabState LedgeGrabState = new();

        public StateController(IStateData _sharedData)
        {
            sharedData = _sharedData;
            sharedData.SharedData.LogElements();
            ChangeState(RunState);
        }
        
        public void Update()
        {
            currentState?.OnStateUpdate(sharedData);
        }

        public void FixedUpdate()
        {
            currentState?.OnStateFixedUpdate(sharedData);
        }

        private void ChangeState(IBaseState _newState)
        {
            if (currentState != null)
            {
                currentState.OnStateExit(sharedData);
                currentState.SwitchState -= ChangeState;
            }
            
            _newState.OnStateEnter(sharedData);
            _newState.SwitchState += ChangeState;

            currentState = _newState;
        }
    }
}