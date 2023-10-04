using Player;
using UnityEngine;

namespace FSM
{
    public class JumpState : IBaseState
    {
        private bool isJumping;
        private bool canJumpAgain;
        
        public void OnStateEnter(IStateData _data)
        {
            var movData = _data.SharedData.Get<MoveVar>("Movement");
            // Perform Jump
            canJumpAgain = true;
            isJumping = true;
            Jump(movData);
        }
        
        public void OnStateUpdate(IStateData _data)
        {
            var movData = _data.SharedData.Get<MoveVar>("Movement");
            var inputData = _data.SharedData.Get<InputData>("input"); 
            
            // this ensures you stay in the JumpState until you are actually grounded again
            if (!movData.IsGrounded) { isJumping = false; }
            
            if (inputData.Jump.WasPressedThisFrame() && movData.IsDoubleJumpUnlocked && canJumpAgain)
            {
                // Perform Double Jump
                canJumpAgain = false;
                isJumping = true;
                Jump(movData);
            }
            if (movData.IsGrounded && !isJumping)
            {
                SwitchState(StateController.RunState);
            }
        }

        public void OnStateFixedUpdate(IStateData _data)
        {
            _data.SharedData.Get<CMC>("cmc").PlayerMove(_data.SharedData.Get<InputData>("input").MoveInput);
            
            if (_data.SharedData.Get<CMC>("cmc").CanLedgeGrab())
            {
                SwitchState(StateController.LedgeGrabState);
            }
        }

        public void OnStateExit(IStateData _data)
        {
            canJumpAgain = true;
            _data.SharedData.Get<MoveVar>("Movement").ExitingSlope = false;
        }

        public StateEvent SwitchState { get; set; }

        private void Jump(MoveVar _jumpData)
        {
            _jumpData.ExitingSlope = true;
            _jumpData.RB.velocity = new (_jumpData.RB.velocity.x, 0f, _jumpData.RB.velocity.z);
            _jumpData.RB.AddForce(Vector3.up * _jumpData.JumpForce, ForceMode.Impulse);
        }
    }
}
