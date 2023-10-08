using UnityEngine;

namespace Player.Pickups
{
    [CreateAssetMenu(fileName = "PickupManager", menuName = "PickupManager", order = 1)]
    public class PickupManagerData : ScriptableObject
    {
        [SerializeField] private Transform     healthPickupLoc;
        [SerializeField] private Pickup        healthPickup;
        
        public Pickup                          HealthPickup => healthPickup;

        public Transform CreateLocs()
        {
            return Instantiate(healthPickupLoc);
        }
    }
}