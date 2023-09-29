﻿using UnityEngine;

namespace FSM
{
    public class WalkState : GroundedState
    {
        protected override void OnEnter()
        {

        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //Debug.Log("WalkStateUpdate");
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