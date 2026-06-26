using UnityEngine;
using Estrellarse.Core;

namespace Estrellarse.UI
{
    public class StateUIController : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject gameplayHUDPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject victoryPanel;

        private void Start()
        {
            if (GameStateManager.Instance != null)
                GameStateManager.Instance.OnStateChanged.AddListener(OnStateChanged);

            SetAllPanelsInactive();
        }

        private void OnDestroy()
        {
            if (GameStateManager.Instance != null)
                GameStateManager.Instance.OnStateChanged.RemoveListener(OnStateChanged);
        }

        public void OnStateChanged(GameState newState)
        {
            SetAllPanelsInactive();

            switch (newState)
            {
                case GameState.Playing:
                    gameplayHUDPanel?.SetActive(true);
                    break;

                case GameState.Paused:
                    gameplayHUDPanel?.SetActive(true);
                    pausePanel?.SetActive(true);
                    break;

                case GameState.GameOver:
                    gameOverPanel?.SetActive(true);
                    break;

                case GameState.Victory:
                    victoryPanel?.SetActive(true);
                    break;
            }
        }

        private void SetAllPanelsInactive()
        {
            gameplayHUDPanel?.SetActive(false);
            pausePanel?.SetActive(false);
            gameOverPanel?.SetActive(false);
            victoryPanel?.SetActive(false);
        }
    }
}