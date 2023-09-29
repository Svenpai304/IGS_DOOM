using UnityEngine;

namespace FSM
{
    public class GroundedState : BaseState
    {
        protected override void OnEnter()
        {
            sc.ChangeState(sc.IdleState);
        }

        protected override void OnUpdate()
        {
            Debug.Log("GroundedStateUpdate");
            // CheckSwitch states
            if (!sc.player.isGrounded)
            {
                sc.ChangeState(sc.InAirState);
            }
            // Handle SlopeMovement
            // Handle Gravity
            sc.player.PlayerMove();
            
        }

        protected override void OnFixedUpdate()
        {
        }

        protected override void OnExit()
        {
            
        }
    }
}