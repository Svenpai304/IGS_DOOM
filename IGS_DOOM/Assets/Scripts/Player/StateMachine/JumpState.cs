using Player;
using UnityEngine;

namespace FSM
{
    public class JumpAction : IBaseState
    {
        public void OnStateEnter(IStateData _data)
        {
            var jumpData = _data.SharedData.Get<MovementVariables>("Movement");
            Debug.Log("Jump");
            jumpData.ExitingSlope = true;
            jumpData.RB.velocity = new (jumpData.RB.velocity.x, 0f, jumpData.RB.velocity.z);
            jumpData.RB.AddForce(Vector3.up * jumpData.JumpForce, ForceMode.Impulse);
        }

        public void OnStateUpdate(IStateData _data)
        {
            if (_data.SharedData.Get<MovementVariables>("Movement").IsGrounded)
            {
                SwitchState(StateController.RunState);
            }
        }

        public void OnStateFixedUpdate(IStateData _data)
        {
        }

        public void OnStateExit(IStateData _data)
        {
        }

        public StateEvent SwitchState { get; set; }
    }
}