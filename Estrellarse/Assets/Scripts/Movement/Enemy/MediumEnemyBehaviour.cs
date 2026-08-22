using UnityEngine;
using UnityEngine.AI;

namespace Estrellarse.Enemy
{
    /// <summary>
    /// Medium enemy gedrag: volgt de speler direct zodra hij in zicht is.
    /// Simpelste AI maar heeft wel een "geheugensysteem" — als hij de
    /// speler kwijtraakt, loopt hij nog even naar de laatste bekende positie
    /// voordat hij stopt. Fundamenteel anders dan patrol of flanken.
    /// </summary>
    public class MediumEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        [SerializeField] private float memoryDuration = 3f;

        public bool IsAggressive { get; private set; }

        private Vector3 _lastKnownPosition;
        private float _memoryTimer;
        private bool _hasMemory;

        public void Tick(Transform self, Transform target, NavMeshAgent agent)
        {
            if (!agent.isOnNavMesh) return;

            PlayerDetector detector = self.GetComponent<PlayerDetector>();
            bool seesPlayer = detector != null && detector.CanSeePlayer;

            if (seesPlayer)
            {
                IsAggressive = true;
                _lastKnownPosition = target.position;
                _hasMemory = true;
                _memoryTimer = memoryDuration;
                agent.SetDestination(target.position);
                return;
            }

            // Geheugen: loopt naar laatste bekende positie
            if (_hasMemory)
            {
                IsAggressive = true;
                _memoryTimer -= Time.deltaTime;
                agent.SetDestination(_lastKnownPosition);

                if (_memoryTimer <= 0f || agent.remainingDistance < 0.5f)
                {
                    _hasMemory = false;
                    IsAggressive = false;
                }
                return;
            }

            IsAggressive = false;
            agent.ResetPath();
        }
    }
}
