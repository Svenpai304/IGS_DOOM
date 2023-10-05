using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player", order = 2)]
    public class PlayerData : ScriptableObject
    {
        public GameObject Player;
        public LayerMask GroundLayer;
        public GameObject Camera;

        [FormerlySerializedAs("PlayerMovementData")] [SerializeField]
        private MoveVar pMoveData;

        public MoveVar PMoveData { get => pMoveData; set => pMoveData = value; }
        public GameObject InstantiatePlayer()
        {
            return Instantiate(Player);
        }

        public GameObject CreateCamera()
        {
            return Instantiate(Camera);
        }
    }

    [Serializable]
    public class MoveVar
    {
        // (Thanks to ID software for showing me this way of organizing variables)
        // https://github.com/id-Software/DOOM-3/tree/master

        // TODO!!!
        // ORGANIZE IN NEW THINGIES EHH, HEADERS!

        [Header("Move Speed")]
        public float                                 AirDrag = 0.4f;
        public float                                 GroundDrag = 3f;
        public float                                 CrouchYScale = .5f;
        public float                                 RunSpeed = 7;
        public float                                 WalkSpeed = 5;
        public float                                 CrouchSpeed = 6;
        
        [Header("Slope Movement")]
        public float                                 MaxSlopeAngle = 31;
        
        [Header("Jumping")]
        public float                                 VaultSpeed = .165f;
        public float                                 AirMultiplier = .4f;
        public float                                 JumpForce = 18f;
        public float                                 StepHeight = .3f;
        public float                                 StepSmooth = .1f;

        // Hidden vars;
        [HideInInspector] public Transform           Orientation;
        [HideInInspector] public Transform           pTransform;
        [HideInInspector] public Transform           StepUpMin;
        [HideInInspector] public Transform           StepUpMax;
        [HideInInspector] public CapsuleCollider     Collider;
        [HideInInspector] public Rigidbody           RB;
        [HideInInspector] public bool                ExitingSlope;
        [HideInInspector] public Vector3             LerpPos;
        [HideInInspector] public bool                IsDoubleJumpUnlocked;
        [HideInInspector] public bool                IsGrounded;
    }
}
