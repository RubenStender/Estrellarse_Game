using UnityEngine;
using Estrellarse.Weapons;

namespace Estrellarse.Enemy
{
    /// <summary>
    /// "Speelt" de trigger voor een enemy, op exact dezelfde manier als
    /// PlayerWeaponController dat doet voor de speler: roept
    /// WeaponHolder.TryFireActive aan. Het enige verschil is de input-bron
    /// (PlayerDetector in plaats van Input.GetKey). De IWeapon/WeaponHolder
    /// laag zelf weet niet en hoeft niet te weten wie de aanroeper is.
    /// </summary>
    [RequireComponent(typeof(WeaponHolder))]
    public class EnemyWeaponController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerDetector detector;
        [SerializeField] private Transform firePoint;

        private WeaponHolder _weaponHolder;

        private void Awake()
        {
            _weaponHolder = GetComponent<WeaponHolder>();

            if (firePoint == null)
                firePoint = transform;
        }

        private void Update()
        {
            if (detector == null || !detector.CanSeePlayer) return;

            // Enemies vuren nooit ADS; dat concept is alleen relevant voor de player.
            _weaponHolder.TryFireActive(firePoint.position, detector.DirectionToPlayer, isADS: false);
        }
    }
}
