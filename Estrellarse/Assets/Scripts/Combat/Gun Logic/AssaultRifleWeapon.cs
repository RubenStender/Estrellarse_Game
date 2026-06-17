namespace Estrellarse.Weapons
{
    /// <summary>
    /// Automatisch wapen: zolang de controller TryFire elke frame blijft
    /// aanroepen (trigger ingedrukt houden), blijft hij vuren met fireRate
    /// als tussenpauze. Gebruikt dezelfde basislogica als Pistol/Shotgun,
    /// alleen met hoge fireRate, lage damage, smalle spread.
    /// </summary>
    public class AssaultRifleWeapon : HitscanWeaponBase
    {
    }
}
