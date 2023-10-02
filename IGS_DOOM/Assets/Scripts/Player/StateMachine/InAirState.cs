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
            Debug.Log(cmc.CanLedgeGrab());
            if (cmc.CanLedgeGrab())
            {
                cmc.LedgeGrab();
            }

            // if can InAirJump > switch to jump state
            // otherwise stay in this InAirState
            //Debug.Log("InAirStateUpdate");
        }

        protected override void OnFixedUpdate()
        {
            
        }

        protected override void OnExit()
        {
            
        }
        

    }
}