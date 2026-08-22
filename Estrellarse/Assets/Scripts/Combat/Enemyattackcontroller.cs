using UnityEngine;

namespace Estrellarse.Enemy
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private PlayerDetector detector;

        private IEnemyAttack _attack;

        private void Awake() => _attack = GetComponent<IEnemyAttack>();

        private void Update()
        {
            if (detector == null || !detector.CanSeePlayer) return;
            if (_attack == null) return;

            float distance = Vector3.Distance(transform.position, detector.PlayerTransform.position);
            if (distance > _attack.AttackRange) return;

            _attack.TryAttack(detector.PlayerTransform);
        }
    }
}