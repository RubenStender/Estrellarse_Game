using System;
using UnityEngine;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// Pistol met headshot multiplier: detecteert of de raycast een
    /// "head" collider raakt en past dan een damage multiplier toe.
    /// Fundamenteel anders dan de basis HitscanWeaponBase — extra
    /// raycast check op een specifieke body-part layer.
    /// </summary>
    public class PistolWeapon : HitscanWeaponBase
    {
        [Header("Headshot")]
        [SerializeField] private LayerMask headLayer;
        [SerializeField] private float headshotMultiplier = 2.5f;

        public event Action OnHeadshot;

        public override void TryFire(Vector3 origin, Vector3 direction, bool isADS)
        {
            if (!CanFire) return;

            if (Physics.Raycast(origin, direction, out RaycastHit headHit, range, headLayer))
            {
                Health health = headHit.collider.GetComponentInParent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage * headshotMultiplier, gameObject);
                    OnHeadshot?.Invoke();
                    OnShoot?.Invoke();
                    reloader?.ConsumeBullet();
                    return;
                }
            }

            base.TryFire(origin, direction, isADS);
        }
    }
}
