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
        private Dictionary<string, GameObject> objs = new ();

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
                objs.Add(child.name, pickups[child.name].GetObj(child));
            }
            AddObserver(_observer);
        }

        private void Update()
        {
            // Very bad solution, BUT this is the only way i could make it work, it was a disaster to debug
            // Because OnDrawGizmos isn't accessible through non MonoBehaviours so i was mostly working blind
            // In the end i got this function that isn't optimized at all and just bad practice. I know this but
            // couldn't really work around it
            foreach (Transform child in HealthLoc)
            {
                Collider[] results = Physics.OverlapSphere(child.position, 3f, LayerMask.GetMask("Player"));
                foreach (var _collider in results)
                {
                    if (_collider.CompareTag("Player"))
                    {
                        if (pickups.TryGetValue(child.name, out var pickup))
                        {
                            CollectPickup(pickup, child.name);
                        }
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
            foreach (Transform child in HealthLoc)
            {
                if (child.name == _key)
                {
                    objs.TryGetValue(_key, out var value);
                    Object.Destroy(value);
                }
            }
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