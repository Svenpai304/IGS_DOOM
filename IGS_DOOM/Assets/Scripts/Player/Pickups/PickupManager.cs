using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.Pickups
{
    // Works with observer pattern (this didnt really come out the way I would've liked. Because this would work better
    // if I could enable OnTriggerEnter on the pickups, but because this isn't a mono behaviour i had some trouble
    // implementing this system so that's why this doesnt work very good)
    
    public class PickupManager
    {
        private static List<IObserver> observers = new ();
        private static Dictionary<string, Pickup> pickups = new ();

        private PickupManagerData pickupData;
        private Transform HealthLoc;
        
        public PickupManager(IObserver _observer)
        {
            GameManager.GlobalUpdate += Update;
            pickupData = Resources.Load<PickupManagerData>("PickupManager");
            HealthLoc = pickupData.CreateLocs();
            AddObserver(_observer);
            foreach (Transform child in HealthLoc)
            {
                pickups.Add(child.name, Object.Instantiate(pickupData.HealthPickup, child));
            }
            AddObserver(_observer);
        }

        private void Update()
        {
            foreach (Transform child in HealthLoc)
            {          
                RaycastHit pickupHit;
                if (Physics.SphereCast(child.position, 10f, child.forward, out pickupHit, 
                        1, LayerMask.GetMask("Player")))
                {
                    if (pickups.TryGetValue(child.name, out var pickup))
                    {
                        CollectPickup(pickup, child.name);
                    }
                }
            }
        }
        
        private void AddObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public static void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        private void CollectPickup(Pickup _pickup, string _key)
        {
            Debug.Log("PickedUp");
            pickups.Remove(_key, out _pickup);
            Object.Destroy(_pickup);
            Notify(_pickup);
        }
        
        private static void Notify(Pickup _pickup)
        {
            foreach (var _observer in observers)
            {
                _observer.OnNotify(_pickup);
            }
        }
    }
}