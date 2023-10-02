using UnityEngine;

namespace FSM
{
    public class InAirState : BaseState
    {
        protected override void OnEnter()
        {
            
        }

        protected override void OnUpdate()
        {
            // Check for ledgeGrab
            // if can ledgeGrab > switch to ledgegrab state

            // if can InAirJump > switch to jump state
            // otherwise stay in this InAirState
            //Debug.Log("InAirStateUpdate");
            if (pData.IsGrounded)
            {
                sc.ChangeState(sc.GroundedState);
            }
        }

        protected override void OnFixedUpdate()
        {
            //LedgeGrab();

            if (pData.CanDoubleJump)
            {
                sc.ChangeState(sc.JumpState);
            }
        }

        protected override void OnExit()
        {
            
        }
        
        /*private void LedgeGrab()
        {
            // Check for wall
            RaycastHit wallHit;
            var bounds = pCollider.bounds;
            Vector3 point1 = bounds.center;
            // point 2 should be lower but not to the ground (we do a separate check for the ground (or if we can split up the complete capsule we can use that),
            // that will be a check to step up certain height ledges)
            // ^^ probs in separate function StepUp()
            Vector3 point2 = new (bounds.center.x, bounds.center.y + playerHalfHeight, bounds.center.z);
            
            // If there is a wall, raycast for ledgedetection
            if (Physics.CapsuleCast(point1, point2, .1f, orientation.forward, out wallHit, 1f, groundLayer))  //Physics.Raycast(pTransform.position + Vector3.up, orientation.forward, out wallHit, 1f, groundLayer))
            {
                Debug.DrawRay(wallHit.point, Vector3.up, Color.blue);
                RaycastHit capsuleHit;
                // NEED TO FIGURE OUT, CAPSULE CAST TO SEE IF THERE IS ENOUGH SPACE FOR THE ENTIRE PLAYER
                //Vector3 capPos1 = wallHit.point + (orientation.forward * radius) + (Vector3.up * (.5f * playerHeight));
                //Vector3 capPos2 = wallHit.point + (orientation.forward * radius) + (Vector3.down * (.5f * playerHeight));
                //Debug.DrawLine(capPos1, capPos2, Color.green);
                Vector3 downRay = wallHit.point + (orientation.forward * playerRadius) + (Vector3.up * (.5f * playerHeight));
                
                // check if there is a ledge, and enough room to stand onto the ledge
                Debug.DrawRay(downRay, Vector3.down, Color.green);
                if (Physics.Raycast(downRay,Vector3.down, out capsuleHit, playerHeight, groundLayer))  //Physics.CapsuleCast(capPos1, capPos2, radius, orientation.forward, out capsuleHit))
                {
                    // lerp the player to the valid position
                    GameManager.Instance.StartCoroutine(LerpToVaultPos(capsuleHit.point, vaultSpeed));
                }
            }
        }*/
    }
}