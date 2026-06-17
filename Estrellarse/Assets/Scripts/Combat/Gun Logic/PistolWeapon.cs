namespace Estrellarse.Weapons
{
    /// <summary>
    /// Enkel schot, geen spread-verlies, geen falloff-curve nodig.
    /// pelletCount staat in de Inspector op 1.
    /// </summary>
    public class PistolWeapon : HitscanWeaponBase
    {
        // Identiek qua logica aan ShotgunWeapon (beide HitscanWeaponBase),
        // maar functioneel een ander wapen door de Inspector waarden:
        // pelletCount = 1, smalle spread, vlakke falloff curve.
    }
}
