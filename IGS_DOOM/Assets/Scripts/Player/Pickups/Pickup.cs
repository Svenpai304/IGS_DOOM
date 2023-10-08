using UnityEngine;

namespace Player.Pickups
{
    // General Pickup class, this gets created as a scripteble object and here you can choose what kind of pickup
    // it is.
    [CreateAssetMenu(fileName = "Pickup", menuName = "Pickup", order = 2)]
    public class Pickup : ScriptableObject
    {
        [SerializeField] private GameObject    obj;
        [SerializeField] private PickupType    type;
        [SerializeField] private int           amount;
        
        public GameObject                      Obj  => obj;
        public PickupType                      Type  => type;
        public int                             Amount => amount;
        
    }

    public enum PickupType
    {
        Health,
        Shield,
        Ammo
    }
}