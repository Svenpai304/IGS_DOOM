using UnityEngine;

namespace FSM
{
    public class RunState : GroundedState
    {
        protected override void OnEnter()
        {
            // Set movement data like movespeed
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            Debug.Log("RunStateUpdate");
            if (sc.player.rb.velocity.magnitude < .1f)
            {
                sc.ChangeState(sc.IdleState);
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