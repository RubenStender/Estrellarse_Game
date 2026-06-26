using UnityEngine;

namespace Estrellarse.Weapons
{
    public interface IAmmoBehaviour
    {
        void OnHit(RaycastHit hit, float damage);
    }
}
