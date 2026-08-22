using UnityEngine;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// Assault Rifle met eerste schot altijd accuraat: de eerste kogel
    /// na een pauze heeft nul spread. Hoe langer je wacht, hoe
    /// accurater je eerste schot. Spread is time-based ipv alleen
    /// ADS-based — echt ander gedrag dan de andere wapens.
    /// </summary>
    public class AssaultRifleWeapon : HitscanWeaponBase
    {
        [Header("First Shot Accuracy")]
        [SerializeField] private float accuracyResetTime = 0.8f;

        private float _lastFireTime;

        public override void TryFire(Vector3 origin, Vector3 direction, bool isADS)
        {
            if (!CanFire) return;

            bool isFirstShot = Time.time - _lastFireTime >= accuracyResetTime;
            float spread = isFirstShot ? 0f : (isADS ? adsSpread : baseSpread);

            firer.FirePellets(origin, direction, pelletCount, spread, range, damage, damageFalloff, ammoBehaviour);
            reloader?.ConsumeBullet();
            OnShoot?.Invoke();

            _lastFireTime = Time.time;
        }
    }
}
