using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace Player
{
    public class CharacterMovementComponent
    {
        
        private Transform orientation;

        
        private Vector3 moveDirection;
        
        private MovementVariables pMoveData;
        private PlayerData pData;
        
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
        
        public CharacterMovementComponent(PlayerData _pData)
        {
            pData = _pData;
            pMoveData = pData.PMoveData;
            
            InitPlayerVariables();
        }
        
        private void InitPlayerVariables()
        {
            playerRadius = pMoveData.Collider.radius;
            playerHeight = pMoveData.Collider.height;
            playerHalfHeight = playerHeight / 2;
            startYScale = pMoveData.pTransform.localScale.y;
        }
        
        public void Update()
        {
            // Grounded check
            // done with CheckCapsule because raycast was very buggy on a little uneven terrain
            var bounds = pMoveData.Collider.bounds;
            Vector3 groundStart = bounds.center;
            Vector3 groundEnd = new (bounds.center.x, bounds.min.y - .2f, bounds.center.z);
            isGrounded = Physics.CheckCapsule(groundStart, groundEnd, playerRadius, pData.GroundLayer);
            Debug.Log(isGrounded);
            // clamp the speed before applying it to the player
            SpeedControl();
            
            // Update the state controller

            if (isGrounded) 
            { 
                Debug.Log("is Grounded");
                pMoveData.RB.drag = pMoveData.GroundDrag;
                //exitingSlope = false;
                readyToJump = true;
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
                pMoveData.RB.drag = pMoveData.AirDrag;
            }
        }
        
        public void Crouch()
        {
            CurrentMoveSpeed = pMoveData.CrouchSpeed;
            
            var localScale = pMoveData.pTransform.localScale;
            localScale = new(localScale.x, crouchYScale, localScale.z);
            pMoveData.pTransform.localScale = localScale;

            // LERP THE SCALE FOR SMOOTHER CROUCH << MAYBE???
            pMoveData.RB.AddForce(Vector3.down * 2.5f, ForceMode.Impulse);
        }

        public void UnCrouch()
        {
            CurrentMoveSpeed = pMoveData.RunSpeed;
            
            var localScale = pMoveData.pTransform.localScale;
            localScale = new(localScale.x, startYScale, localScale.z);
            pMoveData.pTransform.localScale = localScale;
        }

        public void Walk()
        {
            CurrentMoveSpeed = pMoveData.WalkSpeed;
        }
        
        public void Run()
        {
            CurrentMoveSpeed = pMoveData.RunSpeed;
        }
        
        public void Jump()
        {
            Debug.Log("Jump");
            if (coyoteTimeCounter > 0f)
            {
                if (isGrounded || pMoveData.CanDoubleJump)
                {
                    readyToJump = false;
                    PerformJump();

                    pMoveData.CanDoubleJump = !pMoveData.CanDoubleJump;
        
            // FIND A WAY TO RESET THE JUMP VARIABLES. WITH THAT ALSO THE DOUBLE JUMP AND COYOTETIME
            // SLOPE JUMPING WILL BE ENABLED THIS WAY
            // THIS DOESNT WORK BECAUSE THE SYSTEM ISNT RESETTING THE ISEXETING SLOPE VAR
            // PROBS BEST WAY IS VIA THE STATE MACHINE
            //ResetJump();
                }
            }

            /*if (!callbackContext.ReadValueAsButton())
            {
                // THIS NEEDS TO MOVE I THINK :)
                coyoteTimeCounter = 0f;
            }*/
        }

        private void PerformJump()
        {
            pMoveData.ExitingSlope = true;
            pMoveData.RB.velocity = new (pMoveData.RB.velocity.x, 0f, pMoveData.RB.velocity.z);
            pMoveData.RB.AddForce(Vector3.up * pMoveData.JumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            if (isGrounded)
            {
                pMoveData.ExitingSlope = false;
                readyToJump = true;
                pMoveData.CanDoubleJump = false;
            }
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
            Vector3 startPos = pMoveData.pTransform.position;
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

        public void PlayerMove(Vector2 _moveInput)
        {
            moveDirection = pMoveData.Orientation.forward * _moveInput.y + pMoveData.Orientation.right * _moveInput.x;

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

        private bool OnSlope()
        {
            if (Physics.Raycast(pMoveData.pTransform.position, Vector3.down, out slopeHit, playerHalfHeight + .3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }
    }
}