using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player", order = 2)]
    public class PlayerData : ScriptableObject
    {
        public GameObject Player;
        public LayerMask tGroundLayer;
        public GameObject Camera;

        public GameObject InstantiatePlayer()
        {
            return Instantiate(Player);
        }

        public GameObject CreateCamera()
        {
            return Instantiate(Camera);
        }

        public int GroundLayer()
        {
            return LayerMask.GetMask(tGroundLayer.ToString());
        }
    }
}
