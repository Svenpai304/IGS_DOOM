using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{ 
    public abstract class BaseState
    {
        public StateController sc;

        public void OnStateEnter(StateController _sc)
        {
            sc = _sc;
            OnEnter(_sc);
        }
        
        public void OnStateUpdate() { OnUpdate(); }
        
        public void OnStateFixedUpdate() { OnFixedUpdate(); }
        
        public void OnStateExit() { OnExit(); }
        
        protected virtual void OnEnter(StateController _sc) {}
        protected virtual void OnUpdate() {}
        protected virtual void OnFixedUpdate() {}
        protected virtual void OnExit() {}
    }
    
}
