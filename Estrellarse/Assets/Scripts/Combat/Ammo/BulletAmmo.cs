using UnityEngine;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// Standaard kogel-gedrag: brengt schade toe aan een Health component
    /// op het geraakte object, als die bestaat.
    /// </summary>
    public class BulletAmmo : MonoBehaviour, IAmmoBehaviour
    {
        public void OnHit(RaycastHit hit, float damage)
        {
            if (hit.collider.TryGetComponent<Health>(out var health))
                health.TakeDamage(damage, gameObject);
        }
    }
}
