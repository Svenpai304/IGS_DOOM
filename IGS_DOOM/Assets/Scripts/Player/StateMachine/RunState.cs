using UnityEngine;

namespace FSM
{
    public class RunState : GroundedState
    {
        protected override void OnEnter()
        {
            cmc.Run();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //Debug.Log("RunStateUpdate");
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