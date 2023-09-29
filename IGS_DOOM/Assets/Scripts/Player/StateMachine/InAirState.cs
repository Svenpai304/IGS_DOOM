using UnityEngine;

namespace FSM
{
    public class InAirState : BaseState
    {
        protected override void OnEnter()
        {
            
        }

        protected override void OnUpdate()
        {
            // Check for ledgeGrab
            // if can ledgeGrab > switch to ledgegrab state
            
            // if can InAirJump > switch to jump state
            // otherwise stay in this InAirState
            Debug.Log("InAirStateUpdate");
            if (sc.player.isGrounded)
            {
                sc.ChangeState(sc.GroundedState);
            }
        }

        protected override void OnFixedUpdate()
        {
            
        }

        protected override void OnExit()
        {
            
        }
    }
}