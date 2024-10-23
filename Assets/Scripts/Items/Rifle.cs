using Items.ItemInterfaces;

namespace Items
{
    public class Rifle : Gun
    {
        public override WeaponType WeaponType => WeaponType.Rifle;
    }
}