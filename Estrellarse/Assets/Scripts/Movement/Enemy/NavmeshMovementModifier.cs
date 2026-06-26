using UnityEngine;
using UnityEngine.AI;

namespace Estrellarse.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterMotor))]
    public class NavMeshMovementModifier : MonoBehaviour, IMovementModifier
    {
        [Header("Referenties")]
        [SerializeField] private Transform target;

        private NavMeshAgent _agent;
        private IEnemySpeed _speedProfile;

        public bool IsActive => target != null && _agent != null && _agent.isOnNavMesh;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _speedProfile = GetComponent<IEnemySpeed>();

            _agent.updatePosition = false;
            _agent.updateRotation = false;

            if (_speedProfile != null)
            {
                _agent.speed = _speedProfile.MoveSpeed;
                _agent.stoppingDistance = _speedProfile.StoppingDistance;
            }
            else
            {
                Debug.LogWarning("NavMeshMovementModifier: geen IEnemySpeed gevonden op " + gameObject.name, this);
            }
        }

        private void Update()
        {
            if (target == null) return;

            _agent.SetDestination(target.position);

            _agent.nextPosition = transform.position;

            HandleRotation();
        }

        public Vector3 GetVelocityDelta(Vector3 currentVelocity, bool isGrounded)
        {
            if (_agent == null || !_agent.isOnNavMesh) return Vector3.zero;
            if (!_agent.hasPath && !_agent.pathPending) return Vector3.zero;
            if (_agent.pathPending) return Vector3.zero;

            float speed = _speedProfile != null ? _speedProfile.MoveSpeed : _agent.speed;

            Vector3 desiredVelocity = _agent.desiredVelocity;
            desiredVelocity.y = 0f;

            if (desiredVelocity.sqrMagnitude < 0.01f) return Vector3.zero;

            Vector3 currentHorizontal = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
            Vector3 targetVelocity = desiredVelocity.normalized * speed;

            return targetVelocity - currentHorizontal;
        }

        private void HandleRotation()
        {
            if (_agent.desiredVelocity.sqrMagnitude < 0.1f) return;

            float rotSpeed = _speedProfile != null ? _speedProfile.RotationSpeed : 8f;

            Vector3 lookDir = _agent.desiredVelocity;
            lookDir.y = 0f;

            if (lookDir == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            if (_agent != null && newTarget != null)
                _agent.SetDestination(newTarget.position);
        }
    }
}