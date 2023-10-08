using Player.Pickups;

namespace Player
{
    public interface IObserver
    {
        public void OnNotify(Pickup _pickup) {}
    }
}