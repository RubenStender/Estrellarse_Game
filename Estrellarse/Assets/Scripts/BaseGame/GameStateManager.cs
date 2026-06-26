using UnityEngine;
using UnityEngine.Events;

namespace Estrellarse.Core
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }
        [SerializeField] private UnityEvent<GameState> onStateChanged;
        public UnityEvent<GameState> OnStateChanged => onStateChanged;

        private GameState _currentState;
        public GameState CurrentState => _currentState;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() => SetState(GameState.Playing);

        public void ChangeState(GameState newState)
        {
            if (_currentState == newState) return;

            ExitState(_currentState);
            _currentState = newState;
            EnterState(_currentState);

            onStateChanged?.Invoke(_currentState);
        }

        private void SetState(GameState state)
        {
            _currentState = state;
            ApplyStateSettings(state);
            onStateChanged?.Invoke(state);
        }

        private void EnterState(GameState state)
        {
            ApplyStateSettings(state);
        }

        private void ExitState(GameState state)
        {

        }

        private void ApplyStateSettings(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;

                case GameState.GameOver:
                case GameState.Victory:
                    Time.timeScale = 0f;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
            }
        }

        public void PauseGame() => ChangeState(GameState.Paused);
        public void ResumeGame() => ChangeState(GameState.Playing);
        public void TriggerGameOver() => ChangeState(GameState.GameOver);
        public void TriggerVictory() => ChangeState(GameState.Victory);
    }

    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        Victory
    }
}