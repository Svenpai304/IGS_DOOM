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
            if (!_data.SharedData.Get<InputData>("input").IsWalking)
            {
                SwitchState(StateController.RunState);
            }
            if (_data.SharedData.Get<InputData>("input").IsCrouching)
            {
                SwitchState(StateController.CrouchState);
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