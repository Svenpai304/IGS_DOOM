using Player;
using UnityEngine;

namespace FSM
{
    public class RunState : IBaseState
    {
        private CMC cmc;
        public void OnStateEnter(IStateData _data)
        {
            cmc = _data.SharedData.Get<CMC>("cmc");
            cmc.Run();
        }

        public void OnStateUpdate(IStateData _data)
        {
            if (_data.SharedData.Get<InputData>("input").IsWalking)
            {
                SwitchState(StateController.WalkState);
            }

            if (_data.SharedData.Get<InputData>("input").IsCrouching)
            {
                SwitchState(StateController.CrouchState);
            }

            if (_data.SharedData.Get<InputData>("input").Jump.WasPressedThisFrame())
            {
                SwitchState(StateController.JumpState);
            }
        }

        public void OnStateFixedUpdate(IStateData _data)
        {
            cmc.PlayerMove(_data.SharedData.Get<InputData>("input").MoveInput);
        }

        public void OnStateExit(IStateData _data)
        {
            
        }
        
        public StateEvent SwitchState { get; set; }
    }
}