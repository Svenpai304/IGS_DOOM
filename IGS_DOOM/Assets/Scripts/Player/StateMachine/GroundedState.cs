using UnityEngine;

namespace FSM
{
    public class GroundedState : BaseState
    {
        protected override void OnEnter()
        {
            sc.ChangeState(sc.RunState);
        }

        protected override void OnUpdate()
        {
            //Debug.Log("GroundedStateUpdate");
            // CheckSwitch states


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