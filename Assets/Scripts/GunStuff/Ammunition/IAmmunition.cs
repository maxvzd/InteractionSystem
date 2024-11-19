namespace GunStuff.Ammunition
{
    public interface IAmmunition
    {
        bool DecreaseAmmoCount();
        int CurrentBullets { get; }
    }
}