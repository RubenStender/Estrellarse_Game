using UnityEngine;
using UnityEngine.Events;

namespace Estrellarse.Weapons
{
    public class HitscanFirer : MonoBehaviour
    {
        [Header("Hit Detection")]
        [SerializeField] private LayerMask hitMask = ~0;
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private float hitEffectLifetime = 0.5f;

        [Header("Events")]
        public UnityEvent<RaycastHit, float> OnPelletHit;

        public void FirePellets(
            Vector3 origin,
            Vector3 direction,
            int pelletCount,
            float spread,
            float range,
            float damage,
            AnimationCurve damageFalloff,
            IAmmoBehaviour ammoBehaviour)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                Vector3 pelletDir = ApplySpread(direction, spread);

                if (Physics.Raycast(origin, pelletDir, out RaycastHit hit, range, hitMask))
                {
                    float distanceFraction = hit.distance / range;
                    float falloffMultiplier = damageFalloff != null
                        ? damageFalloff.Evaluate(distanceFraction)
                        : 1f;
                    float finalDamage = damage * falloffMultiplier;

                    SpawnHitEffect(hit);
                    ammoBehaviour?.OnHit(hit, finalDamage);
                    OnPelletHit?.Invoke(hit, finalDamage);
                }
            }
        }

        private void SpawnHitEffect(RaycastHit hit)
        {
            if (hitEffectPrefab == null) return;

            GameObject hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitEffect, hitEffectLifetime);
        }

        private Vector3 ApplySpread(Vector3 dir, float spread)
        {
            Vector3 spreadOffset = new Vector3(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                Random.Range(-spread, spread));

            return (dir + spreadOffset).normalized;
        }
    }
}
