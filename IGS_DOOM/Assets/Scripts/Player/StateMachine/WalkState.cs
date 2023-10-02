using UnityEngine;

namespace FSM
{
    public class WalkState : GroundedState
    {
        protected override void OnEnter()
        {
            cmc.Walk();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //Debug.Log("WalkStateUpdate");
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