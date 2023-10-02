using UnityEngine;

namespace FSM
{
    public class RunState : GroundedState
    {
        protected override void OnEnter()
        {
            // Set movement data like movespeed
            pData.CurrentMoveSpeed = pData.RunSpeed;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //Debug.Log("RunStateUpdate");
            if (pData.RB.velocity.magnitude < .1f)
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