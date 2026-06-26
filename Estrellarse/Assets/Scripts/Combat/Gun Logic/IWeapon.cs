using UnityEngine;

namespace Estrellarse.Weapons
{
    public interface IWeapon
    {
        void TryFire(Vector3 origin, Vector3 direction, bool isADS);
        bool CanFire { get; }
    }
}
