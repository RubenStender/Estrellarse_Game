using UnityEngine;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// Smoke-gedrag: spawnt een rookwolk op de hit-locatie, doet bewust
    /// GEEN schade. Dit is een echte gedragsverandering tegenover
    /// BulletAmmo, niet zomaar een aangepaste waarde - het wapen vuurt
    /// nog steeds, maar het effect is fundamenteel anders.
    /// </summary>
    public class SmokeAmmo : MonoBehaviour, IAmmoBehaviour
    {
        [SerializeField] private GameObject smokeCloudPrefab;
        [SerializeField] private float smokeLifetime = 5f;

        public void OnHit(RaycastHit hit, float damage)
        {
            // damage wordt hier bewust genegeerd: smoke doet geen schade,
            // ongeacht wat HitscanFirer berekend heeft.
            if (smokeCloudPrefab == null) return;

            GameObject smoke = Instantiate(smokeCloudPrefab, hit.point, Quaternion.identity);
            Destroy(smoke, smokeLifetime);
        }
    }
}
