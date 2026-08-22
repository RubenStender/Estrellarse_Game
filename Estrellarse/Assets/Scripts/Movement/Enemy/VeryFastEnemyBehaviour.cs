using UnityEngine;
using UnityEngine.AI;

namespace Estrellarse.Enemy
{
    /// <summary>
    /// VeryFast enemy gedrag: sprint direct op de speler af met wisselende
    /// snelheidsbursts. Versnelt periodiek naar een sprint en vertraagt
    /// daarna kort — onvoorspelbaar en moeilijk te raken. Fundamenteel
    /// anders dan de andere types: geen patrol, geen afstand houden,
    /// geen geheugen — pure agressieve rush met tempo-variatie.
    /// </summary>
    public class VeryFastEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        [SerializeField] private float baseSpeed = 8f;
        [SerializeField] private float sprintSpeed = 16f;
        [SerializeField] private float sprintDuration = 0.8f;
        [SerializeField] private float sprintCooldown = 1.5f;

        public bool IsAggressive => true;

        private float _sprintTimer;
        private float _cooldownTimer;
        private bool _isSprinting;

        public void Tick(Transform self, Transform target, NavMeshAgent agent)
        {
            if (!agent.isOnNavMesh) return;

            // Altijd richting de speler, ongeacht of hij in zicht is
            agent.SetDestination(target.position);

            if (_isSprinting)
            {
                agent.speed = sprintSpeed;
                _sprintTimer -= Time.deltaTime;

                if (_sprintTimer <= 0f)
                {
                    _isSprinting = false;
                    _cooldownTimer = sprintCooldown;
                    agent.speed = baseSpeed;
                }
                return;
            }

            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
            {
                _isSprinting = true;
                _sprintTimer = sprintDuration;
            }
        }
    }
}
