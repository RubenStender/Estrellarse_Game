using UnityEngine;

namespace Estrellarse.Enemy
{
    public interface IEnemyAttack
    {
        void TryAttack(Transform target);
        bool CanAttack { get; }
        float AttackRange { get; }
    }
}