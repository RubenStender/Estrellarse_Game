using UnityEngine;

namespace Estrellarse.Enemy
{
    /// <summary>
    /// Detecteert of de player binnen vuurbereik en zichtlijn is. Losse
    /// component zodat hij door meerdere enemy-types hergebruikt kan
    /// worden, los van welk wapen of welke movement-snelheid de enemy heeft.
    /// </summary>
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private float detectionRange = 20f;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private Transform playerTransform; // tijdelijk handmatig instelbaar, later via een PlayerReference singleton/tag-vrije lookup

        public bool CanSeePlayer { get; private set; }
        public Vector3 DirectionToPlayer { get; private set; }
        public Transform PlayerTransform => playerTransform;

        private void Update()
        {
            if (playerTransform == null)
            {
                CanSeePlayer = false;
                return;
            }

            Vector3 toPlayer = playerTransform.position - transform.position;
            float distance = toPlayer.magnitude;

            if (distance > detectionRange)
            {
                CanSeePlayer = false;
                return;
            }

            DirectionToPlayer = toPlayer.normalized;

            bool blocked = Physics.Raycast(transform.position, DirectionToPlayer, distance, obstacleMask);
            CanSeePlayer = !blocked;
        }
    }
}
