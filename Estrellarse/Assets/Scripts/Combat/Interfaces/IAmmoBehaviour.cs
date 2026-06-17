using UnityEngine;

namespace Estrellarse.Weapons
{
    /// <summary>
    /// Bepaalt wat er gebeurt wanneer een pellet/kogel daadwerkelijk iets raakt.
    /// Variaties (Bullet vs Smoke) geven hetzelfde "contract" een ander effect:
    /// schade toebrengen versus een rookwolk spawnen. Dit is een echte
    /// gedragsverandering, geen aanpassing van een waarde.
    /// </summary>
    public interface IAmmoBehaviour
    {
        void OnHit(RaycastHit hit, float damage);
    }
}
