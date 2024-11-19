using Items;
using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace GunStuff.Ammunition
{
    public class Magazine : BaseItem, IAmmunition
    {
        public override IItemProperties ItemProperties => magProperties;
        public override IInteractableProperties Properties => magProperties;
        public Caliber Caliber => caliber;
        public int CurrentBullets => currentBullets;
        public MagazineType MagazineType => magazineType;

        [SerializeField] private ItemProperties magProperties;
        [SerializeField] private int maxBullets;
        [SerializeField] private int currentBullets;
        [SerializeField] private MagazineType magazineType;
        [SerializeField] private Caliber caliber;
        
        public bool DecreaseAmmoCount()
        {
            if (CurrentBullets <= 0) return false;
            
            currentBullets--;
            return true;
        }

        public bool AddBulletToMagazine()
        {
            if (CurrentBullets == maxBullets) return false;
            
            currentBullets++;
            return true;
        }
    }
}