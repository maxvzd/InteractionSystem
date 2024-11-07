using Items.ItemInterfaces;

namespace GunStuff.FireBehaviour
{
    public interface IShotFireBehaviour
    {
        bool Fire(IGun gun);
    }
}