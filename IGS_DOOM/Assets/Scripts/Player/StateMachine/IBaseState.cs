using System.Diagnostics;
using Player;
using UnityEngine;

namespace FSM
{
    public delegate void StateEvent(BaseState _state);
    
    
    // Changes to Interface based Statemachine because Class based didnt work with the StateEvent
    // When passing a state to the event it gave a NULL Exception for some reason
    // (i tried to fix it but nothing seemed to work until I changed it to a interface)
    public interface BaseState
    {
        //protected IStateData data;

        public void OnStateEnter(IStateData _data) {}
        
        public void OnStateUpdate(IStateData _data) {}
        
        public void OnStateFixedUpdate() {}
        
        public void OnStateExit() {}

        public StateEvent SwitchState { get; set; }
    }

    public interface IStateData
    {
        public ScratchPad SharedData { get; }
    }
}
