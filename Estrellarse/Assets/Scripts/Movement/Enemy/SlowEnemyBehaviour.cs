using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Estrellarse.Enemy
{
    /// <summary>
    /// Slow enemy gedrag: patrouilleert tussen waypoints totdat hij de
    /// speler detecteert, daarna valt hij aan. Fundamenteel anders dan
    /// de andere types — heeft een eigen patrol-staat en waypoint-systeem.
    /// Zonder speler in zicht: loopt patrol route.
    /// Met speler in zicht: beweegt direct naar speler toe.
    /// </summary>
    public class SlowEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        [SerializeField] private List<Transform> patrolPoints = new();
        [SerializeField] private float patrolWaitTime = 2f;

        public bool IsAggressive { get; private set; }

        private int _currentPatrolIndex;
        private float _waitTimer;
        private bool _isWaiting;

        public void Tick(Transform self, Transform target, NavMeshAgent agent)
        {
            if (!agent.isOnNavMesh) return;

            PlayerDetector detector = self.GetComponent<PlayerDetector>();
            bool seesPlayer = detector != null && detector.CanSeePlayer;

            if (seesPlayer)
            {
                // Aanvalsmodus: beweeg naar de speler
                IsAggressive = true;
                agent.SetDestination(target.position);
                return;
            }

            // Patrouillemodus
            IsAggressive = false;

            if (patrolPoints.Count == 0) return;

            if (_isWaiting)
            {
                _waitTimer -= Time.deltaTime;
                if (_waitTimer <= 0f)
                {
                    _isWaiting = false;
                    _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Count;
                }
                return;
            }

            Transform waypoint = patrolPoints[_currentPatrolIndex];
            agent.SetDestination(waypoint.position);

            if (agent.remainingDistance < 0.5f && !agent.pathPending)
            {
                _isWaiting = true;
                _waitTimer = patrolWaitTime;
            }
        }
    }
}
