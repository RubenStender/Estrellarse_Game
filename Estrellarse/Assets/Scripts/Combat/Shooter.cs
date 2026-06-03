using UnityEngine;
using UnityEngine.Events;

public class Shooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private LayerMask hitMask = ~0;

    [Header("Spread")]
    [SerializeField] private float baseSpread = 0.02f;
    [SerializeField] private float adsSpread = 0.002f;

    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject hitEffectPrefab;

    [Header("Events")]
    public UnityEvent<Vector3, Vector3> OnHit;
    public UnityEvent OnShoot;
    public UnityEvent OnDryFire;

    private Reloader _reloader;
    private ADS _ads;

    public bool IsADS => _ads != null && _ads.IsAiming;

    private float _nextFireTime;

    private void Awake()
    {
        _reloader = GetComponent<Reloader>();
        _ads = GetComponent<ADS>();

        if (firePoint == null)
            firePoint = transform;
    }

    public void TryShoot(Vector3 origin, Vector3 direction)
    {
        if (Time.time < _nextFireTime) return;

        if (_reloader != null && !_reloader.HasAmmo())
        {
            OnDryFire?.Invoke();
            return;
        }

        _nextFireTime = Time.time + fireRate;
        _reloader?.ConsumeBullet();

        float spread = IsADS ? adsSpread : baseSpread;
        direction = ApplySpread(direction, spread);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, range, hitMask))
        {
            if (hit.collider.TryGetComponent<Health>(out var health))
                health.TakeDamage(damage, gameObject);

            if (hitEffectPrefab != null)
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 0.5f);
            }

            OnHit?.Invoke(hit.point, hit.normal);
        }

        OnShoot?.Invoke();
    }

    public void TryShoot() => TryShoot(firePoint.position, firePoint.forward);

    private Vector3 ApplySpread(Vector3 dir, float spread)
    {
        dir += new Vector3(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread),
            Random.Range(-spread, spread));
        return dir.normalized;
    }

    public void SetDamage(float d) => damage = d;
    public void SetFireRate(float f) => fireRate = f;
}