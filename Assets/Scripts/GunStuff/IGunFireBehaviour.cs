using Items.Weapons;

namespace GunStuff
{
    public interface IGunFireBehaviour
    {
        FireMode FireMode { get; }

        void TriggerUp();
        void Fire(Gun gun);
    }
}