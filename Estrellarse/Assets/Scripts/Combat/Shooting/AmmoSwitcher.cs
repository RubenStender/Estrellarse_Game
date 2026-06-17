using UnityEngine;
using UnityEngine.Events;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// Wisselt het IAmmoBehaviour van een wapen op dit GameObject.
    /// Momenteel een placeholder met een lege/standaard ammo-slot zodat
    /// PlayerWeaponController kan compileren en getest worden; de echte
    /// BulletAmmo/SmokeAmmo implementaties en hun toewijzing komen in de
    /// volgende stap.
    /// </summary>
    public class AmmoSwitcher : MonoBehaviour
    {
        [Tooltip("Elk element moet een component zijn dat IAmmoBehaviour implementeert.")]
        [SerializeField] private MonoBehaviour[] ammoOptions;
        [SerializeField] private HitscanWeaponBase targetWeapon;

        public UnityEvent<int> OnAmmoSwitched;

        private IAmmoBehaviour[] _ammoTypes;
        private int _activeIndex;

        private void Awake()
        {
            _ammoTypes = new IAmmoBehaviour[ammoOptions.Length];

            for (int i = 0; i < ammoOptions.Length; i++)
                _ammoTypes[i] = ammoOptions[i] as IAmmoBehaviour;

            ApplyActiveAmmo();
        }

        public void SwitchAmmo()
        {
            if (_ammoTypes.Length == 0) return;

            _activeIndex = (_activeIndex + 1) % _ammoTypes.Length;
            ApplyActiveAmmo();
            OnAmmoSwitched?.Invoke(_activeIndex);
        }

        private void ApplyActiveAmmo()
        {
            if (targetWeapon == null || _ammoTypes.Length == 0) return;

            targetWeapon.SetAmmoBehaviour(_ammoTypes[_activeIndex]);
        }
    }
}
