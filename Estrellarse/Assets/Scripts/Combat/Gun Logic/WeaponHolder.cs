using UnityEngine;

namespace Estrellarse.Weapons
{
    public class WeaponHolder : MonoBehaviour
    {
        [Tooltip("Elk element moet een component zijn dat IWeapon implementeert (Shotgun, Pistol, Burst, AR, SMG).")]
        [SerializeField] private MonoBehaviour[] weaponOptions;

        private IWeapon[] _weapons;
        private int _activeIndex;

        public IWeapon ActiveWeapon => _weapons != null && _weapons.Length > 0 ? _weapons[_activeIndex] : null;

        private void Awake()
        {
            _weapons = new IWeapon[weaponOptions.Length];

            for (int i = 0; i < weaponOptions.Length; i++)
            {
                _weapons[i] = weaponOptions[i] as IWeapon;

                if (_weapons[i] == null)
                    Debug.LogError($"{weaponOptions[i].name} implementeert geen IWeapon.", this);
            }
        }

        public void EquipNext()
        {
            if (_weapons.Length == 0) return;

            _activeIndex = (_activeIndex + 1) % _weapons.Length;
        }

        public void EquipIndex(int index)
        {
            if (index < 0 || index >= _weapons.Length) return;

            _activeIndex = index;
        }

        public void TryFireActive(Vector3 origin, Vector3 direction, bool isADS)
        {
            ActiveWeapon?.TryFire(origin, direction, isADS);
        }
    }
}
