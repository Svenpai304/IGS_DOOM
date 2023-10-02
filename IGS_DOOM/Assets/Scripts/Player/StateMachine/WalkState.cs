using UnityEngine;

namespace FSM
{
    public class WalkState : GroundedState
    {
        protected override void OnEnter()
        {
            pData.CurrentMoveSpeed = pData.WalkSpeed;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //Debug.Log("WalkStateUpdate");
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