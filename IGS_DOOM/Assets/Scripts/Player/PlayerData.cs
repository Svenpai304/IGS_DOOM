using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player", order = 2)]
    public class PlayerData : ScriptableObject
    {
        public GameObject Player;
        public LayerMask tGroundLayer;
        public string testString;

        public GameObject InstantiatePlayer()
        {
            return Instantiate(Player);
        }

        public LayerMask GroundLayer()
        {
            return tGroundLayer;
        }
    }
}
