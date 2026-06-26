using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Estrellarse.Weapons;

namespace Estrellarse.UI
{
    /// <summary>
    /// Toont wapen-informatie op de HUD door te luisteren naar events van
    /// WeaponHolder, HitscanWeaponBase en AmmoSwitcher. Deze component
    /// polt nooit zelf — hij reageert puur op wat de wapen-laag uitzendt.
    /// Dat is het verschil dat events maken: de HUD weet niks over hoe
    /// wapens werken, hij krijgt alleen een seintje wanneer er iets verandert.
    /// </summary>
    public class WeaponHUD : MonoBehaviour
    {
        [Header("UI Referenties")]
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        [SerializeField] private TextMeshProUGUI ammoTypeText;
        [SerializeField] private Image crosshairImage;

        [Header("Crosshair feedback")]
        [SerializeField] private Color defaultCrosshairColor = Color.white;
        [SerializeField] private Color hitCrosshairColor = Color.red;
        [SerializeField] private float hitFlashDuration = 0.1f;

        [Header("Wapen referentie")]
        [SerializeField] private Reloader reloader;

        private float _hitFlashTimer;

        private void Update()
        {
            UpdateAmmoDisplay();
            UpdateCrosshairFlash();
        }

        // Wordt via de Inspector gekoppeld aan HitscanWeaponBase.OnShoot
        public void OnWeaponFired()
        {
            UpdateAmmoDisplay();
        }

        // Wordt via de Inspector gekoppeld aan HitscanFirer.OnPelletHit
        public void OnPelletHit(RaycastHit hit, float damage)
        {
            if (crosshairImage != null)
            {
                crosshairImage.color = hitCrosshairColor;
                _hitFlashTimer = hitFlashDuration;
            }
        }

        // Wordt via de Inspector gekoppeld aan HitscanWeaponBase.OnDryFire
        public void OnDryFire()
        {
            if (ammoText != null)
                ammoText.color = Color.red;
        }

        // Wordt via de Inspector gekoppeld aan AmmoSwitcher.OnAmmoSwitched
        public void OnAmmoSwitched(int ammoIndex)
        {
            if (ammoTypeText == null) return;

            ammoTypeText.text = ammoIndex == 0 ? "BULLETS" : "SMOKE";
            ammoTypeText.color = ammoIndex == 0 ? Color.white : Color.gray;
        }

        // Wordt via de Inspector gekoppeld aan WeaponHolder wanneer je
        // later een OnWeaponEquipped event toevoegt
        public void OnWeaponEquipped(string weaponName)
        {
            if (weaponNameText != null)
                weaponNameText.text = weaponName;
        }

        private void UpdateAmmoDisplay()
        {
            if (ammoText == null || reloader == null) return;

            ammoText.text = $"{reloader.CurrentAmmo} / {reloader.MaxAmmo}";
            ammoText.color = reloader.HasAmmo() ? Color.white : Color.red;
        }

        private void UpdateCrosshairFlash()
        {
            if (_hitFlashTimer <= 0 || crosshairImage == null) return;

            _hitFlashTimer -= Time.deltaTime;

            if (_hitFlashTimer <= 0)
                crosshairImage.color = defaultCrosshairColor;
        }
    }
}
