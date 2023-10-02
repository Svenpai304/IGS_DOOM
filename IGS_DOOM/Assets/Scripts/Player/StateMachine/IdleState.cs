using UnityEngine;

namespace FSM
{
    public class IdleState : GroundedState
    {
        protected override void OnEnter()
        {
            
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //Debug.Log("IdleStateUpdate");
            if (pData.RB.velocity.magnitude > .1f)
            {
                sc.ChangeState(sc.RunState);
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