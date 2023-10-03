using UnityEngine;

namespace Weapons
{
    public interface IWeaponHolder
    {
        public Transform WeaponTransform { get; set; }
        public Transform CamTransform { get; set; }
        
        // knockBack
        // Recoil?
    }
}