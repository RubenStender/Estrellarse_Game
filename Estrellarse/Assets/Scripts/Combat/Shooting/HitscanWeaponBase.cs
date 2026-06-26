using UnityEngine;
using UnityEngine.Events;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// Gedeelde basis voor alle hitscan-wapens (Shotgun, Pistol, Burst, AR, SMG).
    /// Regelt fire rate timing, reload-check en ADS-check zodat losse wapen
    /// scripts alleen hun eigen pellet/spread/damage instellingen hoeven te
    /// definiëren. Wordt zowel door de player als door enemies gebruikt.
    /// </summary>
    [RequireComponent(typeof(HitscanFirer))]
    public abstract class HitscanWeaponBase : MonoBehaviour, IWeapon
    {
        [Header("Shooting")]
        [SerializeField] protected float damage = 25f;
        [SerializeField] protected float range = 100f;
        [SerializeField] protected float fireRate = 0.5f;

        [Header("Spread")]
        [SerializeField] protected float baseSpread = 0.05f;
        [SerializeField] protected float adsSpread = 0.02f;

        [Header("Pellets")]
        [SerializeField] protected int pelletCount = 1;
        [SerializeField] protected AnimationCurve damageFalloff = AnimationCurve.Linear(0f, 1f, 1f, 1f);

        [Header("Events")]
        [SerializeField] private UnityEvent onShoot;
        [SerializeField] private UnityEvent onDryFire;

        public UnityEvent OnShoot => onShoot;
        public UnityEvent OnDryFire => onDryFire;

        protected HitscanFirer firer;
        protected Reloader reloader;
        protected IAmmoBehaviour ammoBehaviour;

        private float _nextFireTime;

        public virtual bool CanFire => Time.time >= _nextFireTime && (reloader == null || reloader.HasAmmo());

        protected virtual void Awake()
        {
            firer = GetComponent<HitscanFirer>();
            reloader = GetComponent<Reloader>();
            ammoBehaviour = GetComponent<IAmmoBehaviour>();
        }

        public virtual void TryFire(Vector3 origin, Vector3 direction, bool isADS)
        {
            if (Time.time < _nextFireTime) return;

            if (reloader != null && !reloader.HasAmmo())
            {
                OnDryFire?.Invoke();
                return;
            }

            _nextFireTime = Time.time + fireRate;
            reloader?.ConsumeBullet();

            float spread = isADS ? adsSpread : baseSpread;
            firer.FirePellets(origin, direction, pelletCount, spread, range, damage, damageFalloff, ammoBehaviour);

            OnShoot?.Invoke();
        }
        public void SetAmmoBehaviour(IAmmoBehaviour behaviour) => ammoBehaviour = behaviour;
    }
}