using System;
using UnityEngine;

namespace Estrellarse.Enemy
{
    public class MeleeAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private float damage = 20f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private LayerMask playerMask;

        public event Action OnAttack;

        private float _nextAttackTime;

        public bool CanAttack => Time.time >= _nextAttackTime;
        public float AttackRange => attackRange;

        public void TryAttack(Transform target)
        {
            if (!CanAttack) return;

            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, playerMask);

            foreach (Collider hit in hits)
            {
                Health health = hit.GetComponentInParent<Health>();
                if (health == null) continue;

                health.TakeDamage(damage, gameObject);
                OnAttack?.Invoke();
                break;
            }

            _nextAttackTime = Time.time + attackCooldown;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}