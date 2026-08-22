using System;
using UnityEngine;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// SMG met spin-up: de eerste shots doen minder damage en hebben
    /// meer spread. Hoe langer je vuurt, hoe nauwkeuriger en sterker.
    /// Damage en spread veranderen dynamisch op basis van hoe lang
    /// je al aan het schieten bent.
    /// </summary>
    public class SMGWeapon : HitscanWeaponBase
    {
        [Header("Spin-up")]
        [SerializeField] private float spinUpTime = 1.5f;
        [SerializeField] private float minDamageMultiplier = 0.4f;
        [SerializeField] private float spinDownSpeed = 2f;

        private float _spinProgress;

        public event Action<float> OnSpinProgressChanged;

        private void Update()
        {
            _spinProgress = Mathf.Max(0f, _spinProgress - Time.deltaTime * spinDownSpeed);
            OnSpinProgressChanged?.Invoke(_spinProgress);
        }

        public override void TryFire(Vector3 origin, Vector3 direction, bool isADS)
        {
            if (!CanFire) return;

            _spinProgress = Mathf.Min(1f, _spinProgress + Time.deltaTime / spinUpTime);

            float damageMultiplier = Mathf.Lerp(minDamageMultiplier, 1f, _spinProgress);
            float spreadMultiplier = Mathf.Lerp(2f, 1f, _spinProgress);

            firer.FirePellets(
                origin,
                direction,
                pelletCount,
                baseSpread * spreadMultiplier,
                range,
                damage * damageMultiplier,
                damageFalloff,
                ammoBehaviour);

            reloader?.ConsumeBullet();
            OnShoot?.Invoke();
        }
    }
}
