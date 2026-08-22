using UnityEngine;

namespace Estrellarse.Enemy
{
    /// <summary>
    /// Contract voor enemy AI-gedrag. Elke implementatie heeft een
    /// fundamenteel andere manier van reageren op de speler — niet
    /// alleen andere waarden, maar andere logica en staat.
    /// NavMeshMovementModifier roept Tick() elke Update aan.
    /// </summary>
    public interface IEnemyBehaviour
    {
        void Tick(Transform self, Transform target, UnityEngine.AI.NavMeshAgent agent);
        bool IsAggressive { get; }
    }
}
