using UnityEngine;
using Player;

namespace FSM
{
    public class CrouchState : IBaseState
    {
        private CMC cmc;
        public void OnStateEnter(IStateData _data)
        {
            cmc = _data.SharedData.Get<CMC>("cmc");
            cmc.Crouch();
        }

        public void OnStateUpdate(IStateData _data)
        {
            if (!_data.SharedData.Get<InputData>("input").IsCrouching)
            {
                SwitchState(StateController.RunState);
            }
        }

        public void OnStateFixedUpdate(IStateData _data)
        {
            cmc.PlayerMove(_data.SharedData.Get<InputData>("input").MoveInput);
        }

        public void OnStateExit(IStateData _data)
        {
            cmc.UnCrouch();
        }
        
        public StateEvent SwitchState { get; set; }
    }
}