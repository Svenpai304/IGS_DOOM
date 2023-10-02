using UnityEngine;
    
namespace FSM
{
    public class JumpState : GroundedState
    {
        protected override void OnEnter()
        {
            cmc.Jump();
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
    }
}