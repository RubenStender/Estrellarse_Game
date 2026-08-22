using System;
using UnityEngine;

namespace Estrellarse.Core
{
    public class DashPickup : MonoBehaviour
    {
        [SerializeField] private GameObject pickupVisual;
        [SerializeField] private float respawnTime = 0f;

        public event Action OnPickedUp;

        private bool _collected;

        private void OnTriggerEnter(Collider other)
        {
            if (_collected) return;
            if (!other.TryGetComponent<PlayerIdentifier>(out _)) return;
            if (other.TryGetComponent<Dash>(out _)) return; // al opgepakt

            other.gameObject.AddComponent<Dash>();

            if (other.TryGetComponent<CharacterMotor>(out var motor))
                motor.RefreshModifiers();

            OnPickedUp?.Invoke();
            _collected = true;

            if (pickupVisual != null)
                pickupVisual.SetActive(false);

            if (respawnTime > 0f)
                Invoke(nameof(Respawn), respawnTime);
        }

        private void Respawn()
        {
            _collected = false;
            if (pickupVisual != null)
                pickupVisual.SetActive(true);
        }
    }
}