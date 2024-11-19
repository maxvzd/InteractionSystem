using GunStuff.Ammunition;
using UnityEngine;

namespace Items.Weapons
{
    public abstract class MagazineGun : Gun, IMagazineGun
    {
        [SerializeField] private MagazineType acceptedMagazine;
        public MagazineType AcceptedMagazine => acceptedMagazine;
        
        public bool ReloadMagazine(Magazine magazine)
        {
            if (magazine.MagazineType != AcceptedMagazine) return false;

            Ammunition = magazine;
            
            return true;
        } 
    }
}