namespace Items.ItemSlots
{
    public interface IWeapon : IEquipabble
    {
        WeaponType WeaponType { get; }
    }

    public enum WeaponType
    {
        Rifle,
        Pistol
    }
}