using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player", order = 2)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField]
        private GameObject player;
        [SerializeField]
        private GameObject camera;
        [SerializeField]
        private MoveVar pMoveData;
        
        public LayerMask GroundLayer;
        public MoveVar PMoveData => pMoveData;
        
        public GameObject InstantiatePlayer()
        {
            return Instantiate(player);
        }

        public GameObject CreateCamera()
        {
            return Instantiate(camera);
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
        public float                               AirDrag              = 0.4f;
        public float                               GroundDrag           = 3f;
        public float                               CrouchYScale         = 0.5f;
        public float                               RunSpeed             = 7;
        public float                               WalkSpeed            = 5;
        public float                               CrouchSpeed          = 6;
        
        [Header("Slope Movement")]
        public float                               MaxSlopeAngle        = 31;
        
        [Header("Jumping")]
        public float                               VaultSpeed           = 0.165f;
        public float                               AirMultiplier        = 0.4f;
        public float                               JumpForce            = 18f;
        public float                               StepHeight           = 0.3f;
        public float                               StepSmooth           = 0.1f;
        public float                               MeleeDistance        = 4;
        public float                               GloryKillDistance    = 4;
        public int                                 MeleeDmg             = 4;
        
        // Hidden vars;
        [HideInInspector] public Transform         Orientation;
        [HideInInspector] public Transform         pTransform;
        [HideInInspector] public Transform         StepUpMin;
        [HideInInspector] public Transform         StepUpMax;
        [HideInInspector] public CapsuleCollider   Collider;
        [HideInInspector] public Rigidbody         RB;
        [HideInInspector] public bool              ExitingSlope;
        [HideInInspector] public Vector3           LerpPos;
        [HideInInspector] public bool              IsDoubleJumpUnlocked;
        [HideInInspector] public bool              IsGrounded;
    }
}
