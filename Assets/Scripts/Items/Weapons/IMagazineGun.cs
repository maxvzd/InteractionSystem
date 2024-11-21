using GunStuff.Ammunition;
using Items.ItemInterfaces;

namespace Items.Weapons
{
    public interface IMagazineGun : IGun
    {
        MagazineType AcceptedMagazine { get; }
        bool ReloadMagazine(Magazine magazine);
    }
}