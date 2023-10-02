using System.Collections;
using UnityEngine;

namespace Player
{
    public class CharacterMovementComponent
    {
        
        private Transform orientation;

        
        private Vector3 moveDirection;
        
        private MovementVariables pMoveData;
        
        public bool isGrounded;
        private LayerMask groundLayer;


        private RaycastHit slopeHit;

        public float CurrentMoveSpeed = 7;



        private float startYScale;
        private float playerRadius;

        private float coyoteTimeCounter;
        
        // can initialize from constructor
        private float playerHeight;
        private float playerHalfHeight;

        private float jumpCoolDown;
        private bool readyToJump;

        private bool isWalking;
        
        
        // PlayerMovement variables (changable)

        private float coyoteTime = 3.2f;
        private float airMultiplier = .4f;
        private float jumpForce = 18f;
        private float doubleJumpForce = 15f;
        private float groundDrag = 5f;
        private float airDrag = 0f;
        private float crouchYScale = .5f;
        public float WalkSpeed = 5;
        public float RunSpeed = 7;
        public float CrouchSpeed = 6;
        private float maxSlopeAngle = 31;
        private float vaultSpeed = .165f;
        
        CharacterMovementComponent(MovementVariables _pMoveData)
        {
            pMoveData = _pMoveData;
        }
        
        private void LedgeGrab()
        {
            // Check for wall
            RaycastHit wallHit;
            var bounds = pMoveData.Collider.bounds;
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
        }

        private IEnumerator LerpToVaultPos(Vector3 _targetPos, float _duration)
        {
            float time = 0;
            Vector3 startPos = pTransform.position;
            Vector3 targetPos = new (_targetPos.x, _targetPos.y + playerHalfHeight, _targetPos.z);
            while (time < _duration)
            {
                // move the player to the targetpos
                pMoveData.RB.MovePosition(Vector3.Lerp(startPos, targetPos, time / _duration));
                time += Time.deltaTime;
                yield return null;
            }
            // Make sure the player is at the target pos at the end (lerp isn't exact)
            pMoveData.RB.MovePosition(targetPos);
        }

        private void FixedUpdate()
        {
            stateController.FixedUpdate();
            // MOVE THIS TO A STATE PLS
            //LedgeGrab();
            
            PlayerMove();
        }

        public void PlayerMove()
        {
            moveDirection = pMoveData.Orientation.forward * moveInput.y + pMoveData.Orientation.right * moveInput.x;

            if (OnSlope() && !pMoveData.ExitingSlope)
            {
                pMoveData.RB.AddForce(GetSlopeMovementDirection() * (CurrentMoveSpeed * 20f), ForceMode.Force);

                if (pMoveData.RB.velocity.y > 0)
                {
                    pMoveData.RB.AddForce(Vector3.down * 100, ForceMode.Force);
                }
            }
            
            if (isGrounded && !OnSlope())
                pMoveData.RB.AddForce(moveDirection.normalized * (CurrentMoveSpeed * 10f), ForceMode.Force);
            else if (!isGrounded)
                pMoveData.RB.AddForce(moveDirection.normalized * (CurrentMoveSpeed * 10f * airMultiplier), ForceMode.Force);

            pMoveData.RB.useGravity = !OnSlope();
        }

        private void SpeedControl()
        {
            if (OnSlope() && !pMoveData.ExitingSlope)
            {
                if (pMoveData.RB.velocity.magnitude > CurrentMoveSpeed)
                {
                    pMoveData.RB.velocity = pMoveData.RB.velocity.normalized * CurrentMoveSpeed;
                }
            }
            else
            {
                Vector3 flatVel = new (pMoveData.RB.velocity.x, 0f, pMoveData.RB.velocity.z);

                if (flatVel.magnitude > CurrentMoveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * CurrentMoveSpeed;
                    pMoveData.RB.velocity = new (limitedVel.x, pMoveData.RB.velocity.y, limitedVel.z);
                }
            }
        }
        
        private Vector3 GetSlopeMovementDirection()
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }



        private IEnumerator HandleCrouch(float _targetHeight, float _duration)
        {
            float time = 0;
            Vector3 targetHeight = new(pTransform.localScale.x, _targetHeight, pTransform.localScale.z);
            while (time < _duration)
            {
                // Lerp the playerScale to the targetHeight
                pTransform.localScale = Vector3.Lerp(pTransform.localScale, targetHeight, time / _duration);
                time += Time.deltaTime;
                yield return null;
            }
            // Make sure the player is at the target Height at the end
            pTransform.localScale = targetHeight;
        }

        private bool OnSlope()
        {
            if (Physics.Raycast(pTransform.position, Vector3.down, out slopeHit, playerHalfHeight + .3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private void PerformJump()
        {
            pMoveData.ExitingSlope = true;
            pMoveData.RB.velocity = new (pMoveData.RB.velocity.x, 0f, pMoveData.RB.velocity.z);
            pMoveData.RB.AddForce(Vector3.up * (pMoveData.CanDoubleJump ? doubleJumpForce : jumpForce), ForceMode.Impulse);
        }

        /*private void ResetJump()
        {
            if (isGrounded)
            {
                exitingSlope = false;
                readyToJump = true;
                doubleJump = false;
                exitingSlope = false;
            }
        }*/
    }
}