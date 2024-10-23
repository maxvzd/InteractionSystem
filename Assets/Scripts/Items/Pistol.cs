using Items.ItemInterfaces;

namespace Items
{
    public class Pistol : Gun
    {
        public override WeaponType WeaponType => WeaponType.Pistol;
    }
}