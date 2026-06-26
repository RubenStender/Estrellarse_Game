using UnityEngine;
using Estrellarse.Weapons;

namespace Estrellarse.Enemy
{
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

            _weaponHolder.TryFireActive(firePoint.position, detector.DirectionToPlayer, isADS: false);
        }
    }
}
