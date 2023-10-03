using Player;
using UnityEngine;

namespace FSM
{
    // State machine
    // I looked into Pushdown automata but this isnt really neceserry for this system
    // https://github.com/TS696/PdStateMachine/tree/master << only real recourse i found (in unity)
    // For maybe a complexer system that needs state history it might be a better fit
    // But DOOM 4 doesnt really have that complex of a movement system so you dont need to know the prev
    // state or anything, so that's why i went for a normal state machine

    public class StateController
    {
        private IBaseState currentState;

        // State Definitions
        public static WalkState WalkState = new();
        public static RunState RunState = new ();
        public static CrouchState CrouchState = new();
        public static JumpState JumpState = new();

        // Action Defenitions
        public static LedgeGrabAction LedgeGrabAction = new();

        // Input Data
        private IStateData sharedData;

        public StateController(IStateData _sharedData)
        {
            sharedData = _sharedData;
            sharedData.SharedData.LogElements();
            ChangeState(RunState);
        }
        
        public void Update()
        {
            currentState?.OnStateUpdate(sharedData);
            Debug.Log(currentState);
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