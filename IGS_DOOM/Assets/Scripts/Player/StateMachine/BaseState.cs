using Player;
using UnityEngine;

namespace FSM
{ 
    public abstract class BaseState
    {
        protected StateController sc;
        protected CharacterMovementComponent cmc;
        public void OnStateEnter(StateController _sc, CharacterMovementComponent _cmc)
        {
            sc = _sc;
            cmc = _cmc;
            OnEnter();
        }
        
        public void OnStateUpdate() { OnUpdate(); }
        
        public void OnStateFixedUpdate() { OnFixedUpdate(); }
        
        public void OnStateExit() { OnExit(); }
        
        // Virtual functions that are overridden by the base class
        protected virtual void OnEnter() {}
        protected virtual void OnUpdate() {}
        protected virtual void OnFixedUpdate() {}
        protected virtual void OnExit() {}
    }
    
}
