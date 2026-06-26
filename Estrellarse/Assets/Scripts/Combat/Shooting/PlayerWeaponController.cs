using UnityEngine;
using Estrellarse.Weapons;

namespace Estrellarse.Player
{
    [RequireComponent(typeof(WeaponHolder))]
    public class PlayerWeaponController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private ADS ads;

        [Header("Input")]
        [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
        [SerializeField] private KeyCode switchWeaponKey = KeyCode.Q;
        [SerializeField] private KeyCode switchAmmoKey = KeyCode.F;

        private WeaponHolder _weaponHolder;
        private AmmoSwitcher _ammoSwitcher;

        private void Awake()
        {
            _weaponHolder = GetComponent<WeaponHolder>();
            _ammoSwitcher = GetComponent<AmmoSwitcher>();

            if (firePoint == null)
                firePoint = transform;
        }

        private void Update()
        {
            HandleFiring();
            HandleWeaponSwitch();
            HandleAmmoSwitch();
        }

        private void HandleFiring()
        {
            bool isHeld = Input.GetKey(fireKey);
            if (!isHeld) return;

            bool isADS = ads != null && ads.IsAiming;
            _weaponHolder.TryFireActive(firePoint.position, firePoint.forward, isADS);
        }

        private void HandleWeaponSwitch()
        {
            if (Input.GetKeyDown(switchWeaponKey))
                _weaponHolder.EquipNext();
        }

        private void HandleAmmoSwitch()
        {
            if (_ammoSwitcher == null) return;

            if (Input.GetKeyDown(switchAmmoKey))
                _ammoSwitcher.SwitchAmmo();
        }
    }
}
