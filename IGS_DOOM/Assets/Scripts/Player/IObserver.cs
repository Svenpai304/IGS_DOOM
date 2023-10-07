using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public interface IObserver
    {
        public void OnNotify(Pickup _pickup) {}
    }

    public class PickupManager
    {
        private static List<IObserver> observers = new ();
        private List<Pickup> pickups = new ();
        public static void AddObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public static void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }
        
        public void AddPickup(Pickup pickup)
        {
            pickups.Add(pickup);
        }

        public void CollectPickup(Pickup _pickup)
        {
            pickups.Remove(_pickup);
            Notify(_pickup);
        }
        
        private void Notify(Pickup _pickup)
        {
            foreach (var _observer in observers)
            {
                _observer.OnNotify(_pickup);
            }
        }
    }
    
    [CreateAssetMenu(fileName = "Pickup", menuName = "Pickup", order = 2)]
    public class Pickup : ScriptableObject
    {
        [SerializeField] private GameObject    obj;
        [SerializeField] private PickupType    type;
        [SerializeField] private int           amount;
        
        public GameObject                      Obj   => obj;
        public PickupType                      Type  => type;
        public int                             Amount => amount;

        [SerializeField] private SphereCollider col;

        public SphereCollider Col
        {
            get => col;
            set => col = value;
        }
    }

    public enum PickupType
    {
        Health,
        Shield,
        Ammo
    }
}