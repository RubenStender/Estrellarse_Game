using UnityEngine;

namespace Estrellarse.Core
{
    public class WinZone : MonoBehaviour
    {
        [SerializeField] private GameObject winCanvas;
        [SerializeField] private GameObject uiCanvas;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerIdentifier>(out _)) return;

            if (winCanvas != null)
                winCanvas.SetActive(true);
                uiCanvas.SetActive(false);

            Time.timeScale = 0f;
        }
    }
}