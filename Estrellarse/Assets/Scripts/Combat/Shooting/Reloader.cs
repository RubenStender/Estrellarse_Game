using UnityEngine;
using UnityEngine.Events;

namespace Estrellarse.Weapons
{
    public class Reloader : MonoBehaviour
    {
        [SerializeField] private int maxAmmo = 30;
        [SerializeField] private float reloadTime = 1.5f;

        public UnityEvent OnReloadStart;
        public UnityEvent OnReloadComplete;
        public UnityEvent OnAmmoChanged;

        private int _currentAmmo;
        private bool _isReloading;

        public int CurrentAmmo => _currentAmmo;
        public int MaxAmmo => maxAmmo;

        private void Awake() => _currentAmmo = maxAmmo;

        public bool HasAmmo() => _currentAmmo > 0 && !_isReloading;

        public void ConsumeBullet()
        {
            if (_currentAmmo <= 0) return;

            _currentAmmo--;
            OnAmmoChanged?.Invoke();

            if (_currentAmmo == 0)
                StartReload();
        }

        public void TryReload() => StartReload();

        public void StartReload()
        {
            if (_isReloading || _currentAmmo == maxAmmo) return;

            _isReloading = true;
            OnReloadStart?.Invoke();
            Invoke(nameof(FinishReload), reloadTime);
        }

        private void FinishReload()
        {
            _currentAmmo = maxAmmo;
            _isReloading = false;
            OnReloadComplete?.Invoke();
            OnAmmoChanged?.Invoke();
        }
    }
}