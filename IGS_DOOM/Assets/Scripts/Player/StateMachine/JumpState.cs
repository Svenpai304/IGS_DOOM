using UnityEngine;
    
namespace FSM
{
    public class JumpState : GroundedState
    {
        protected override void OnEnter()
        {
            PerformJump();
        }

        protected override void OnUpdate()
        {

        }

        protected override void OnFixedUpdate()
        {
            
        }

        protected override void OnExit()
        {
            
        }
        
        private void PerformJump()
        {
            pData.ExitingSlope = true;
            pData.RB.velocity = new (pData.RB.velocity.x, 0f, pData.RB.velocity.z);
            pData.RB.AddForce(Vector3.up * (pData.CanDoubleJump ? pData.DoubleJumpForce : pData.JumpForce), ForceMode.Impulse);
            sc.ChangeState(sc.InAirState);
        }
    }
}