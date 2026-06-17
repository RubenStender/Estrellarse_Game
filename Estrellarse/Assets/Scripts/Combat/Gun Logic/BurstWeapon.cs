using System.Collections;
using UnityEngine;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// Vuurt een vast aantal schoten snel achter elkaar bij één TryFire-call,
    /// in plaats van één schot per call zoals Pistol/Shotgun. Dit is een
    /// echt ander vuurgedrag, niet alleen een andere fireRate waarde.
    /// </summary>
    public class BurstWeapon : HitscanWeaponBase
    {
        [Header("Burst")]
        [SerializeField] private int burstCount = 3;
        [SerializeField] private float burstInterval = 0.08f;

        private bool _isBursting;

        public override bool CanFire => base.CanFire && !_isBursting;

        public override void TryFire(Vector3 origin, Vector3 direction, bool isADS)
        {
            if (!CanFire) return;

            StartCoroutine(FireBurst(origin, direction, isADS));
        }

        private IEnumerator FireBurst(Vector3 origin, Vector3 direction, bool isADS)
        {
            _isBursting = true;

            for (int i = 0; i < burstCount; i++)
            {
                if (reloader != null && !reloader.HasAmmo())
                {
                    OnDryFire?.Invoke();
                    break;
                }

                reloader?.ConsumeBullet();

                float spread = isADS ? adsSpread : baseSpread;
                firer.FirePellets(origin, direction, pelletCount, spread, range, damage, damageFalloff, ammoBehaviour);
                OnShoot?.Invoke();

                yield return new WaitForSeconds(burstInterval);
            }

            _isBursting = false;
        }
    }
}
