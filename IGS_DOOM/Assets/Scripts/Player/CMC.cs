using Unity.VisualScripting;
using UnityEngine;

//-----------------------------------------------------------------------------------
// CharacterMovementComponent, This handles all the Generic movement logic that cant
// be handled in the states themselves. Here you can find the generic Move function
// and the functions that get referenced when certain logic defines behaviour only
// accessible through here like setting the movement speed
//-----------------------------------------------------------------------------------

namespace Player
{
    public class CMC
    {
        private MoveVar pMoveData;
        private PlayerData pData;



        // can initialize from constructor
        private float playerHeight;
        private float playerRadius;

        private float currentMoveSpeed;
        
        // PlayerMovement variables (changable)
        
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

        private void Update()
        {
            pMoveData.IsGrounded = GroundCheck();
            
            // clamp the speed before applying it to the player
            SpeedControl();
            
            if (!OnSlope())
            {
                StepUp();
            }
        }
        
        private void FixedUpdate()
        {

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
                return true;
            }
            // Set InAir variables
            pMoveData.RB.drag = pMoveData.AirDrag;
            
            return false;
        }
        
        public void Run() { currentMoveSpeed = pMoveData.RunSpeed; }
        public void Walk() { currentMoveSpeed = pMoveData.WalkSpeed; }
        public void Crouch() { currentMoveSpeed = pMoveData.CrouchSpeed; }
        
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
                pMoveData.Orientation.TransformDirection(-1.5f, 0, 1)
            };
            
            foreach (var direction in directions)
            {
                RaycastHit isSlope;
                if (Physics.Raycast(pMoveData.StepUpMin.position, direction, out isSlope,
                        CalculateStepDistance(), pData.GroundLayer))
                {
                    // Just a check to make sure you go Smoothly on and off slopes
                    if (Vector3.Angle(Vector3.up, isSlope.normal) < pMoveData.MaxSlopeAngle) { return; }
                    
                    if (!Physics.Raycast(pMoveData.StepUpMax.position, direction,  
                            CalculateStepDistance(), pData.GroundLayer))
                    {
                        pMoveData.RB.position += new Vector3(0f, +pMoveData.StepSmooth, 0f);
                    }
                } 
            }
        }

        private float CalculateStepDistance()
        {
            return pMoveData.RB.velocity.magnitude / 13;
        }

        public void PlayerMove(Vector2 _moveInput)
        {
            // Get the direction to move in
            Vector3 moveDirection = pMoveData.Orientation.forward * _moveInput.y + pMoveData.Orientation.right * _moveInput.x;

            // Move Player
            if (OnSlope() && !pMoveData.ExitingSlope)
            {
                pMoveData.RB.AddForce(GetSlopeMovementDirection(moveDirection) * (currentMoveSpeed * 20f), ForceMode.Force);

                if (pMoveData.RB.velocity.y > 0)
                {
                    pMoveData.RB.AddForce(Vector3.down * 100, ForceMode.Force);
                }
            }

            if (pMoveData.IsGrounded && !OnSlope())
            {
                pMoveData.RB.AddForce(moveDirection.normalized * (currentMoveSpeed * 10f), ForceMode.Force);
            }
            else if (!pMoveData.IsGrounded)
            {
                pMoveData.RB.AddForce(moveDirection.normalized * 
                                      (currentMoveSpeed * 10f * pMoveData.AirMultiplier), ForceMode.Force);
            }

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
        
        private Vector3 GetSlopeMovementDirection(Vector3 _moveDirection)
        {
            return Vector3.ProjectOnPlane(_moveDirection, slopeHit.normal).normalized;
        }

        private RaycastHit slopeHit;
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