using UnityEngine;
using Player;

namespace FSM
{
    public class CrouchState : IBaseState
    {
        private float startYScale;
        public void OnStateEnter(IStateData _data)
        {
            var movData = _data.SharedData.Get<MoveVar>("Movement");

            // Set the movement speed in the movement Component
            _data.SharedData.Get<CMC>("cmc").Crouch();
            startYScale = movData.pTransform.localScale.y;
            
            var localScale = movData.pTransform.localScale;
            localScale = new(localScale.x, movData.CrouchYScale, localScale.z);
            movData.pTransform.localScale = localScale;
            movData.RB.AddForce(Vector3.down * 2.5f, ForceMode.Impulse);
        }

        public void OnStateUpdate(IStateData _data)
        {
            var inputData = _data.SharedData.Get<InputData>("input");
            if (!inputData.IsCrouching || inputData.Jump.WasPerformedThisFrame())
            {
                SwitchState(StateController.RunState);
                Debug.Log("Switch to standing state");
            }
        }

        public void OnStateFixedUpdate(IStateData _data)
        {
            _data.SharedData.Get<CMC>("cmc").PlayerMove(_data.SharedData.Get<InputData>("input").MoveInput);
        }

        public void OnStateExit(IStateData _data)
        {
            var movData = _data.SharedData.Get<MoveVar>("Movement");
            
            var localScale = movData.pTransform.localScale;
            localScale = new(localScale.x, startYScale, localScale.z);
            movData.pTransform.localScale = localScale;
        }
        
        public StateEvent SwitchState { get; set; }
    }
}