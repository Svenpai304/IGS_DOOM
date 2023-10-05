using Player;

namespace FSM
{
    public class WalkState : IBaseState
    {
        private CMC cmc;
        public void OnStateEnter(IStateData _data)
        {
            cmc = _data.SharedData.Get<CMC>("cmc");
            cmc.Walk();
        }

        public void OnStateUpdate(IStateData _data)
        {
            var inputData = _data.SharedData.Get<InputData>("input");
            if (!inputData.IsWalking)
            {
                SwitchState(StateController.RunState);
            }
            if (inputData.IsCrouching)
            {
                SwitchState(StateController.CrouchState);
            }
            if (inputData.Jump.WasPressedThisFrame())
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