using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class CMC
    {
        private Vector3 moveDirection;
        
        private MoveVar pMoveData;
        private PlayerData pData;

        private bool isGrounded;

        private RaycastHit slopeHit;
        
        private float coyoteTimeCounter;
        
        // can initialize from constructor
        private float playerHeight;
        private float playerRadius;
        private float startYScale;

        private float jumpCoolDown;
        private bool isReadyToJump;

        private bool isWalking;
        
        private float currentMoveSpeed = 7;
        
        // PlayerMovement variables (changable)

        private float coyoteTime = 3.2f;


        public CMC(PlayerData _pData)
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

            pMoveData.StepUpMax.position = new(pMoveData.StepUpMax.position.x, pMoveData.StepHeight, 
                pMoveData.StepUpMax.position.z);

        }
        
        public void Update()
        {
            pMoveData.IsGrounded = GroundCheck();
            
            // clamp the speed before applying it to the player
            SpeedControl();
        }
        private void FixedUpdate()
        {
            if (!OnSlope())
            {
                StepUp();
            }
        }

        private bool GroundCheck()
        {
            var bounds = pMoveData.Collider.bounds;
            Vector3 groundStart = bounds.center;
            Vector3 groundEnd = new (bounds.center.x, bounds.min.y, bounds.center.z);
            if (Physics.CheckCapsule(groundStart, groundEnd, playerRadius, pData.GroundLayer))
            {
                // Set Grounded variables
                pMoveData.RB.drag = pMoveData.GroundDrag;
                //exitingSlope = false;
                isReadyToJump = true;
                coyoteTimeCounter = coyoteTime;
                return true;
            }
            
            // Set InAir variables
            coyoteTimeCounter -= Time.deltaTime;
            pMoveData.RB.drag = pMoveData.AirDrag;
            
            return false;
        }

        
        public void Run() { currentMoveSpeed = pMoveData.RunSpeed; }
        public void Walk() { currentMoveSpeed = pMoveData.WalkSpeed; }
        public void Crouch() { currentMoveSpeed = pMoveData.CrouchSpeed; }
        
        public void Jump()
        {
            if (coyoteTimeCounter > 0f)
            {
                if (pMoveData.IsGrounded || pMoveData.IsDoubleJumpUnlocked)
                {
                    isReadyToJump = false;
                    //PerformJump();

                    pMoveData.IsDoubleJumpUnlocked = !pMoveData.IsDoubleJumpUnlocked;
        
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
        
        public bool CanLedgeGrab()
        {
            var bounds = pMoveData.Collider.bounds;
            Vector3 point1 = bounds.center;
            Vector3 point2 = new (bounds.center.x, bounds.center.y + playerHeight/2, bounds.center.z);
            // ^^ point 2 should be lower but not to the ground (There is a seperate check for stepup)
            
            // If there is a wall, raycast for ledgedetection
            if (Physics.CapsuleCast(point1, point2, .1f, pMoveData.Orientation.forward, out var wallHit, 
                    1f, pData.GroundLayer))
            {
                RaycastHit capsuleHit;
                // NEED TO FIGURE OUT, CAPSULE CAST TO SEE IF THERE IS ENOUGH SPACE FOR THE ENTIRE PLAYER
                //Vector3 capPos1 = wallHit.point + (orientation.forward * radius) + (Vector3.up * (.5f * playerHeight));
                //Vector3 capPos2 = wallHit.point + (orientation.forward * radius) + (Vector3.down * (.5f * playerHeight));
                //Debug.DrawLine(capPos1, capPos2, Color.green);
                Vector3 downRay = wallHit.point + pMoveData.Orientation.forward * playerRadius + Vector3.up * (.5f * playerHeight);
                
                // check if there is a ledge, and enough room to stand onto the ledge
                Debug.DrawRay(downRay, Vector3.down, Color.green);
                if (Physics.Raycast(downRay,Vector3.down, out capsuleHit, playerHeight, pData.GroundLayer))  //Physics.CapsuleCast(capPos1, capPos2, radius, orientation.forward, out capsuleHit))
                {
                    pMoveData.LerpPos = capsuleHit.point;
                    return true;
                }
            }
            
            return false;
        }

        private void StepUp()
        {
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
            return pMoveData.RB.velocity.magnitude / 10;
        }

        public void PlayerMove(Vector2 _moveInput)
        {
            // Get the direction to move in
            moveDirection = pMoveData.Orientation.forward * _moveInput.y + pMoveData.Orientation.right * _moveInput.x;

            // Move Player
            if (OnSlope() && !pMoveData.ExitingSlope)
            {
                pMoveData.RB.AddForce(GetSlopeMovementDirection() * (currentMoveSpeed * 20f), ForceMode.Force);

                if (pMoveData.RB.velocity.y > 0)
                {
                    pMoveData.RB.AddForce(Vector3.down * 100, ForceMode.Force);
                }
            }
            
            if (pMoveData.IsGrounded && !OnSlope())
                pMoveData.RB.AddForce(moveDirection.normalized * (currentMoveSpeed * 10f), ForceMode.Force);
            else if (!pMoveData.IsGrounded)
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
            if (Physics.Raycast(pMoveData.pTransform.position, Vector3.down, out slopeHit, playerHeight/2 + .3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < pMoveData.MaxSlopeAngle && angle != 0;
            }

            return false;
        }
    }
}