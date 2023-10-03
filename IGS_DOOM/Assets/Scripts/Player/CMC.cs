using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace Player
{
    public class CharacterMovementComponent
    {
        private Vector3 moveDirection;
        
        private MovementVariables pMoveData;
        private PlayerData pData;

        private bool isGrounded;

        private RaycastHit slopeHit;

        

        private float coyoteTimeCounter;
        
        // can initialize from constructor
        private float playerHeight;
        private float playerHalfHeight;
        private float playerRadius;
        private float startYScale;

        private float jumpCoolDown;
        private bool isReadyToJump;

        private bool isWalking;
        
        private float currentMoveSpeed = 7;
        
        // PlayerMovement variables (changable)

        private float coyoteTime = 3.2f;
        private float crouchYScale = .5f;

        public CharacterMovementComponent(PlayerData _pData)
        {
            pData = _pData;
            pMoveData = pData.PMoveData;

            GameManager.GlobalFixedUpdate += FixedUpdate;
            GameManager.GlobalUpdate += Update;
            InitPlayerVariables();
        }
        
        private void InitPlayerVariables()
        {
            playerRadius = pMoveData.Collider.radius;
            playerHeight = pMoveData.Collider.height;
            playerHalfHeight = playerHeight / 2;
            startYScale = pMoveData.pTransform.localScale.y;

            pMoveData.StepUpMax.position = new(pMoveData.StepUpMax.position.x, pMoveData.StepHeight, 
                pMoveData.StepUpMax.position.z);

        }
        
        public void Update()
        {
            Debug.Log(currentMoveSpeed);
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
                pMoveData.RB.drag = pMoveData.GroundDrag;
                //exitingSlope = false;
                isReadyToJump = true;
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
                pMoveData.RB.drag = pMoveData.AirDrag;
            }
        }

        private void FixedUpdate()
        {
            StepUp();
        }
        
        public void Crouch()
        {
            currentMoveSpeed = pMoveData.CrouchSpeed;
            
            var localScale = pMoveData.pTransform.localScale;
            localScale = new(localScale.x, crouchYScale, localScale.z);
            pMoveData.pTransform.localScale = localScale;

            // LERP THE SCALE FOR SMOOTHER CROUCH << MAYBE???
            pMoveData.RB.AddForce(Vector3.down * 2.5f, ForceMode.Impulse);
        }

        public void UnCrouch()
        {
            // Need to check if you can Uncrouch, currently if you crouch under a object you will clip into its
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            currentMoveSpeed = pMoveData.RunSpeed;
            
            var localScale = pMoveData.pTransform.localScale;
            localScale = new(localScale.x, startYScale, localScale.z);
            pMoveData.pTransform.localScale = localScale;
        }

        public void Walk()
        {
            currentMoveSpeed = pMoveData.WalkSpeed;
        }
        
        public void Run()
        {
            currentMoveSpeed = pMoveData.RunSpeed;
        }
        
        public void Jump()
        {
            if (coyoteTimeCounter > 0f)
            {
                if (isGrounded || pMoveData.CanDoubleJump)
                {
                    isReadyToJump = false;
                    //PerformJump();

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

        private void ResetJump()
        {
            if (isGrounded)
            {
                pMoveData.ExitingSlope = false;
                isReadyToJump = true;
                pMoveData.CanDoubleJump = false;
            }
        }

        private RaycastHit capsuleHit;
        public bool CanLedgeGrab()
        {
            RaycastHit wallHit;
            var bounds = pMoveData.Collider.bounds;
            Vector3 point1 = bounds.center;
            // point 2 should be lower but not to the ground (we do a separate check for the ground (or if we can split up the complete capsule we can use that),
            // that will be a check to step up certain height ledges)
            // ^^ probs in separate function StepUp()
            Vector3 point2 = new (bounds.center.x, bounds.center.y + playerHalfHeight, bounds.center.z);
            
            // If there is a wall, raycast for ledgedetection
            if (Physics.CapsuleCast(point1, point2, .1f, pMoveData.Orientation.forward, out wallHit, 
                    1f, pData.GroundLayer))
            {
                Debug.DrawRay(wallHit.point, Vector3.up, Color.blue);
                
                // NEED TO FIGURE OUT, CAPSULE CAST TO SEE IF THERE IS ENOUGH SPACE FOR THE ENTIRE PLAYER
                //Vector3 capPos1 = wallHit.point + (orientation.forward * radius) + (Vector3.up * (.5f * playerHeight));
                //Vector3 capPos2 = wallHit.point + (orientation.forward * radius) + (Vector3.down * (.5f * playerHeight));
                //Debug.DrawLine(capPos1, capPos2, Color.green);
                Vector3 downRay = wallHit.point + (pMoveData.Orientation.forward * playerRadius) + (Vector3.up * (.5f * playerHeight));
                
                // check if there is a ledge, and enough room to stand onto the ledge
                Debug.DrawRay(downRay, Vector3.down, Color.green);
                if (Physics.Raycast(downRay,Vector3.down, out capsuleHit, playerHeight, pData.GroundLayer))  //Physics.CapsuleCast(capPos1, capPos2, radius, orientation.forward, out capsuleHit))
                {
                    return true;
                }
            }

            return false;
        }
        
        public void LedgeGrab()
        {
            GameManager.Instance.StartCoroutine(LerpToVaultPos(capsuleHit.point, pMoveData.VaultSpeed));
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

        private void StepUp()
        {
            //Debug.DrawRay(pMoveData.StepUpMin.position, 
            //    pMoveData.Orientation.TransformDirection(Vector3.forward) * CalculateStepDistance(), Color.green);
            
            // Make 3 rays, 1 straight and 2 at a 45 and -45 degree angle respectively
            Vector3[] directions =
            {
                pMoveData.Orientation.TransformDirection(Vector3.forward),
                pMoveData.Orientation.TransformDirection(1.5f, 0, 1),
                pMoveData.Orientation.TransformDirection(-1.5f, 0, 1),
            };
            
            foreach (var direction in directions)
            {
                if (Physics.Raycast(pMoveData.StepUpMin.position, direction, CalculateStepDistance()))
                {
                    if (Physics.Raycast(pMoveData.StepUpMin.position, direction, CalculateStepDistance()))
                    {
                        pMoveData.RB.position -= new Vector3(0f, -pMoveData.StepSmooth, 0f);
                    }
                }
            }
        }

        private float CalculateStepDistance()
        {
            // Calculate step distance based on the magnitude of the velocity
            // The faster you go the further away it will cast
            // This gives a smoother feel
            return pMoveData.RB.velocity.magnitude / 10;
        }

        public void PlayerMove(Vector2 _moveInput)
        {
            moveDirection = pMoveData.Orientation.forward * _moveInput.y + pMoveData.Orientation.right * _moveInput.x;

            if (OnSlope() && !pMoveData.ExitingSlope)
            {
                pMoveData.RB.AddForce(GetSlopeMovementDirection() * (currentMoveSpeed * 20f), ForceMode.Force);

                if (pMoveData.RB.velocity.y > 0)
                {
                    pMoveData.RB.AddForce(Vector3.down * 100, ForceMode.Force);
                }
            }
            
            if (isGrounded && !OnSlope())
                pMoveData.RB.AddForce(moveDirection.normalized * (currentMoveSpeed * 10f), ForceMode.Force);
            else if (!isGrounded)
                pMoveData.RB.AddForce(moveDirection.normalized * (currentMoveSpeed * 10f * pMoveData.AirMultiplier), ForceMode.Force);

            pMoveData.RB.useGravity = !OnSlope();
        }

        private void SpeedControl()
        {
            if (OnSlope() && !pMoveData.ExitingSlope)
            {
                if (pMoveData.RB.velocity.magnitude > currentMoveSpeed)
                {
                    pMoveData.RB.velocity = pMoveData.RB.velocity.normalized * currentMoveSpeed;
                }
            }
            else
            {
                Vector3 flatVel = new (pMoveData.RB.velocity.x, 0f, pMoveData.RB.velocity.z);

                if (flatVel.magnitude > currentMoveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * currentMoveSpeed;
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
                return angle < pMoveData.MaxSlopeAngle && angle != 0;
            }

            return false;
        }
    }
}