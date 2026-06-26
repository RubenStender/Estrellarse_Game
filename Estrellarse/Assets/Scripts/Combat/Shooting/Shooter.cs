using UnityEngine;
using UnityEngine.Events;
using Estrellarse.Weapons;

public class Shooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private LayerMask hitMask = ~0;

    [Header("Shotgun")]
    [SerializeField] private int pelletCount = 8;
    [SerializeField] private AnimationCurve damageFalloff = AnimationCurve.EaseInOut(0f, 1f, 1f, 0.2f);

    [Header("Spread")]
    [SerializeField] private float baseSpread = 0.1f;
    [SerializeField] private float adsSpread = 0.05f;

    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject hitEffectPrefab;

    [Header("Events")]
    public UnityEvent<Vector3, Vector3> OnHit;
    public UnityEvent OnShoot;
    public UnityEvent OnDryFire;

    private Reloader _reloader;
    private ADS _ads;
    private Camera _camera;

    public bool IsADS => _ads != null && _ads.IsAiming;

    private float _nextFireTime;

    private void Awake()
    {
        _reloader = GetComponent<Reloader>();
        _ads = GetComponent<ADS>();
        _camera = Camera.main;

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

        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 pelletDir = ApplySpread(direction, spread);

            if (Physics.Raycast(origin, pelletDir, out RaycastHit hit, range, hitMask))
            {
                float distanceFraction = hit.distance / range;
                float falloffMultiplier = damageFalloff.Evaluate(distanceFraction);
                float finalDamage = damage * falloffMultiplier;

                Health health = hit.collider.GetComponentInParent<Health>();
                Debug.Log($"Hit: {hit.collider.gameObject.name} | Health gevonden: {health != null}");
                if (health != null)
                    health.TakeDamage(finalDamage, gameObject);

                if (hitEffectPrefab != null)
                {
                    GameObject hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(hitEffect, 0.5f);
                }

                OnHit?.Invoke(hit.point, hit.normal);
            }
        }

        OnShoot?.Invoke();
    }

    public void TryShoot()
    {
        Vector3 origin = _camera != null ? _camera.transform.position : firePoint.position;
        Vector3 direction = _camera != null ? _camera.transform.forward : firePoint.forward;
        TryShoot(origin, direction);
    }

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