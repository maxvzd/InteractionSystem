using GunStuff.Ammunition;
using UnityEngine;

namespace Items.Weapons
{
    public abstract class MagazineGun : Gun, IMagazineGun
    {
        public MagazineType AcceptedMagazine => acceptedMagazine;
        
        [SerializeField] private MagazineType acceptedMagazine;
        
        public bool ReloadMagazine(Magazine magazine)
        {
            if (magazine.MagazineType != AcceptedMagazine) return false;
            if (Ammunition is Magazine currentMag && magazine == currentMag) return false;

            Ammunition = magazine;
            
            return true;
        } 
    }
}