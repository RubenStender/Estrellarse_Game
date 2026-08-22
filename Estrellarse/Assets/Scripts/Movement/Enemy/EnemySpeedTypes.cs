using UnityEngine;

namespace Estrellarse.Enemy
{
    public class SlowEnemySpeed : MonoBehaviour, IEnemySpeed
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float stoppingDistance = 2f;
        [SerializeField] private float rotationSpeed = 5f;

        public float MoveSpeed => moveSpeed;
        public float StoppingDistance => stoppingDistance;
        public float RotationSpeed => rotationSpeed;
    }

    public class MediumEnemySpeed : MonoBehaviour, IEnemySpeed
    {
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float stoppingDistance = 2f;
        [SerializeField] private float rotationSpeed = 8f;

        public float MoveSpeed => moveSpeed;
        public float StoppingDistance => stoppingDistance;
        public float RotationSpeed => rotationSpeed;
    }

    public class FastEnemySpeed : MonoBehaviour, IEnemySpeed
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float stoppingDistance = 10f;
        [SerializeField] private float rotationSpeed = 12f;

        public float MoveSpeed => moveSpeed;
        public float StoppingDistance => stoppingDistance;
        public float RotationSpeed => rotationSpeed;
    }

    public class VeryFastEnemySpeed : MonoBehaviour, IEnemySpeed
    {
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float stoppingDistance = 1f;
        [SerializeField] private float rotationSpeed = 20f;

        public float MoveSpeed => moveSpeed;
        public float StoppingDistance => stoppingDistance;
        public float RotationSpeed => rotationSpeed;
    }
}