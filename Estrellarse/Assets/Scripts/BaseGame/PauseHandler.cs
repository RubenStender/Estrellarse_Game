using UnityEngine;
using Estrellarse.Core;

namespace Estrellarse.Player
{
    /// <summary>
    /// Vangt Escape-input op en schakelt tussen Playing en Paused.
    /// Losse component zodat input-afhandeling niet in GameStateManager zit.
    /// </summary>
    public class PauseHandler : MonoBehaviour
    {
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (GameStateManager.Instance == null) return;

            GameState current = GameStateManager.Instance.CurrentState;

            if (current == GameState.Playing)
                GameStateManager.Instance.PauseGame();
            else if (current == GameState.Paused)
                GameStateManager.Instance.ResumeGame();
        }
    }
}
