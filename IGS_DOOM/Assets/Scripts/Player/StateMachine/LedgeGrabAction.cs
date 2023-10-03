using Player;
using UnityEngine;

namespace FSM
{
    public class LedgeGrabAction : StateAction
    {
        public override void Execute(IStateData _data)
        {
            Debug.Log("grab Ledge");
        }
    }
}