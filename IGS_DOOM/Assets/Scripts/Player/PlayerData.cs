using System;
using UnityEngine;

//-----------------------------------------------------------------------------------
// PlayerData class is used for initializing all the variables the player needs for
// movement and other related stuff. This also acts as global accesses for the
// classes that need it
//-----------------------------------------------------------------------------------

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

        [Header("General Movement")]
        public float                               AirMultiplier        = 0.1f;
        public float                               AirDrag              = 0.4f;
        public float                               GroundDrag           = 3f;

        public float                               RunSpeed             = 7;
        public float                               WalkSpeed            = 5;
        public float                               CrouchSpeed          = 6;
        
        public float                               StepHeight           = 0.3f;
        public float                               StepSmooth           = 0.1f;
        
        public float                               MaxSlopeAngle        = 31;
        public float                               CrouchYScale         = 0.5f;
        
        [Header("Jumping and Parkour")]
        public float                               JumpForce            = 18f;
        public float                               VaultSpeed           = 0.165f;
        
        public int                                 MeleeDmg             = 4;
        public float                               MeleeDistance        = 4;
        public float                               GloryKillDistance    = 4;
        
        // Hidden vars Used for accessing public info but that isnt initialized by the user but by the system itself
        [HideInInspector] public Transform         Orientation;
        [HideInInspector] public Transform         pTransform;
        [HideInInspector] public Transform         StepUpMin;
        [HideInInspector] public Transform         StepUpMax;
        [HideInInspector] public CapsuleCollider   Collider;
        [HideInInspector] public Rigidbody         RB;
        [HideInInspector] public Vector3           LerpPos;
        [HideInInspector] public bool              ExitingSlope;
        [HideInInspector] public bool              IsDoubleJumpUnlocked;
        [HideInInspector] public bool              IsGrounded;
    }
}
