using UnityEngine;

/// <summary>
/// Aim Down Sights. Lerps FOV and reduces sensitivity while aiming.
/// Shooter reads IsAiming to tighten spread automatically.
/// Works standalone — remove this component to have hipfire-only.
/// </summary>
public class ADS : MonoBehaviour
{
    [Header("ADS Settings")]
    [SerializeField] private float adsFOV          = 40f;
    [SerializeField] private float normalFOV        = 70f;
    [SerializeField] private float adsLerpSpeed     = 10f;
    [SerializeField] private float adsSensitivity   = 0.4f; // Multiplier on mouse look

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    public bool IsAiming           { get; private set; }
    public float SensitivityScale  => IsAiming ? adsSensitivity : 1f;

    // Called by InputBridge
    public void SetAiming(bool aiming) => IsAiming = aiming;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        if (playerCamera == null) return;

        float targetFOV = IsAiming ? adsFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView, targetFOV, adsLerpSpeed * Time.deltaTime);
    }
}
