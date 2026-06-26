using UnityEngine;

namespace Estrellarse.Weapons
{
    public class BulletAmmo : MonoBehaviour, IAmmoBehaviour
    {
        public void OnHit(RaycastHit hit, float damage)
        {
            Health health = hit.collider.GetComponentInParent<Health>();
            if (health != null)
                health.TakeDamage(damage, gameObject);
        }
    }
}