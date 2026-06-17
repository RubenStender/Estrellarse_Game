using UnityEngine;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// Contract voor "iets dat kan vuren". Weet niets over wie de trigger
    /// indrukt (player input of AI) - dat zit in een IWeaponController.
    /// Hierdoor kan exact dezelfde implementatie op de player en op
    /// enemies gebruikt worden.
    /// </summary>
    public interface IWeapon
    {
        void TryFire(Vector3 origin, Vector3 direction, bool isADS);
        bool CanFire { get; }
    }
}
