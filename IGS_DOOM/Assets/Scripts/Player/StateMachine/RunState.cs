using Player;

namespace FSM
{
    public class RunState : IBaseState
    {
        public void OnStateEnter(IStateData _data)
        {
            _data.SharedData.Get<CMC>("cmc").Run();
        }

        public void OnStateUpdate(IStateData _data)
        {
            var inputData = _data.SharedData.Get<InputData>("input");
            
            if (inputData.IsWalking)
            {
                SwitchState(StateController.WalkState);
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
            _data.SharedData.Get<CMC>("cmc").PlayerMove(_data.SharedData.Get<InputData>("input").MoveInput);
        }

        public void OnStateExit(IStateData _data)
        {
            
        }
        
        public StateEvent SwitchState { get; set; }
    }
}