using UnityEngine;
using UnityEngine.AI;

namespace Estrellarse.Enemy
{
    /// <summary>
    /// Fast enemy gedrag: houdt afstand en schiet op de speler vanuit
    /// een veilige positie. Als de speler te dichtbij komt, wijkt hij
    /// actief terug. Fundamenteel anders — beweegt NIET naar de speler
    /// toe maar zoekt een schietpositie op afstand.
    /// </summary>
    public class FastEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        [SerializeField] private float preferredRange = 12f;
        [SerializeField] private float retreatRange = 6f;
        [SerializeField] private float strafeSpeed = 3f;

        public bool IsAggressive { get; private set; }

        private float _strafeTimer;
        private int _strafeDirection = 1;

        public void Tick(Transform self, Transform target, NavMeshAgent agent)
        {
            if (!agent.isOnNavMesh) return;

            PlayerDetector detector = self.GetComponent<PlayerDetector>();
            bool seesPlayer = detector != null && detector.CanSeePlayer;

            if (!seesPlayer)
            {
                IsAggressive = false;
                agent.ResetPath();
                return;
            }

            IsAggressive = true;
            float distance = Vector3.Distance(self.position, target.position);

            if (distance < retreatRange)
            {
                // Te dichtbij: wijk terug
                Vector3 retreatDir = (self.position - target.position).normalized;
                Vector3 retreatPos = self.position + retreatDir * preferredRange;
                agent.SetDestination(retreatPos);
                return;
            }

            if (distance > preferredRange * 1.5f)
            {
                // Te ver: kom dichterbij maar niet té dichtbij
                Vector3 toPlayer = (target.position - self.position).normalized;
                Vector3 approachPos = target.position - toPlayer * preferredRange;
                agent.SetDestination(approachPos);
                return;
            }

            // Op goede afstand: strafe zijwaarts
            _strafeTimer -= Time.deltaTime;
            if (_strafeTimer <= 0f)
            {
                _strafeDirection *= -1;
                _strafeTimer = Random.Range(1f, 2.5f);
            }

            Vector3 right = Vector3.Cross(
                (target.position - self.position).normalized, Vector3.up) * _strafeDirection;
            agent.SetDestination(self.position + right * strafeSpeed);
        }
    }
}
