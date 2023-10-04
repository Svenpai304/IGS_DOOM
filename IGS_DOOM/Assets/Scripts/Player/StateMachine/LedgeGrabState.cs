using System.Collections;
using Player;
using UnityEngine;

namespace FSM
{
    public class LedgeGrabState : IBaseState
    {
        public void OnStateEnter(IStateData _data)
        {
            var movData = _data.SharedData.Get<MoveVar>("Movement");
            GameManager.Instance.StartCoroutine(LerpToVaultPos(movData.LerpPos, movData.VaultSpeed, movData));
        }
        
        public void OnStateExit(IStateData _data)
        {
        }

        public StateEvent SwitchState { get; set; }

        private IEnumerator LerpToVaultPos(Vector3 _targetPos, float _duration, MoveVar _LedgeData)
        {
            float time = 0;
            Vector3 startPos = _LedgeData.pTransform.position;
            Vector3 targetPos = new (_targetPos.x, _targetPos.y + _LedgeData.Collider.height/2, _targetPos.z);
            
            while (time < _duration)
            {
                // move the player to the targetpos
                _LedgeData.RB.MovePosition(Vector3.Lerp(startPos, targetPos, time / _duration));
                time += Time.deltaTime;
                yield return null;
            }
            
            // Make sure the player is at the target pos at the end (lerp isn't exact)
            _LedgeData.RB.MovePosition(targetPos);

            // This state has now finished so switch
            SwitchState(StateController.RunState);
        }
    }
}