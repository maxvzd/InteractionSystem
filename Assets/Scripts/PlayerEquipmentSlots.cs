using Items.ItemInterfaces;

public struct PlayerEquipmentSlots
{
    public IBackpack Backpack { get; }
    public IWeapon Weapon { get; }
    
    public PlayerEquipmentSlots(IBackpack backpack, IWeapon weapon)
    {
        Backpack = backpack;
        Weapon = weapon;
    }
}