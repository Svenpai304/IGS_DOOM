using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player", order = 2)]
    public class PlayerData : ScriptableObject
    {
        public GameObject Player;

        public GameObject InstantiatePlayer()
        {
            return Instantiate(Player);
        }
    }
}
