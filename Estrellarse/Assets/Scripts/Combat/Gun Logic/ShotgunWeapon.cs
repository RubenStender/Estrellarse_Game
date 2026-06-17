namespace Estrellarse.Weapons
{
    /// <summary>
    /// Veel pellets, brede spread, sterke falloff over afstand.
    /// Ondersteunt ammo-swap (Bullet/Smoke) via AmmoSwitcher.
    /// </summary>
    public class ShotgunWeapon : HitscanWeaponBase
    {
        // Gebruikt de standaard TryFire/CanFire uit HitscanWeaponBase.
        // De shotgun-identiteit komt volledig uit de Inspector waarden:
        // hoge pelletCount, brede baseSpread, sterke damageFalloff curve.
    }
}
