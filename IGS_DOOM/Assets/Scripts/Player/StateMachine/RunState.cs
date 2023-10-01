using UnityEngine;

namespace FSM
{
    public class RunState : GroundedState
    {
        protected override void OnEnter()
        {
            // Set movement data like movespeed
            sc.player.CurrentMoveSpeed = sc.player.RunSpeed;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //Debug.Log("RunStateUpdate");
            if (sc.player.rb.velocity.magnitude < .1f)
            {
                sc.ChangeState(sc.IdleState);
            }
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        protected override void OnExit()
        {
            
        }
    }
}