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
            //Debug.Log("GroundedStateUpdate");
            // CheckSwitch states
            if (!pData.IsGrounded)
            {
                sc.ChangeState(sc.InAirState);
            }
            
            
            // Handle SlopeMovement
            // Handle Gravity
            
        }

        protected override void OnFixedUpdate()
        {
            //Debug.Log("fixedUpodate move");
            
        }

        protected override void OnExit()
        {
            
        }
    }
}