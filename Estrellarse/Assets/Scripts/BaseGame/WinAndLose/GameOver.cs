using UnityEngine;

namespace Estrellarse.Core
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private GameObject uiCanvas;

        public void OnPlayerDeath()
        {
            if (gameOverCanvas != null)
                gameOverCanvas.SetActive(true);

            if (uiCanvas != null)
                uiCanvas.SetActive(false);

            Time.timeScale = 0f;
        }
    }
}