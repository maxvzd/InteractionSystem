using GunStuff.Ammunition;

namespace Items.Weapons
{
    public interface IMagazineGun
    {
        MagazineType AcceptedMagazine { get; }
        bool ReloadMagazine(Magazine magazine);
    }
}