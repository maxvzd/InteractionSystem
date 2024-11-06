using Items.Weapons;

namespace GunStuff
{
    public interface IFireMode
    {
        FireMode FireMode { get; }

        void TriggerUp();
        bool Fire();
    }
}