using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class BaseState
    {
        public virtual void OnEnter() {}
        
        public virtual void OnUpdate() {}
       
        public virtual void OnExit() {}
    }
    
}
