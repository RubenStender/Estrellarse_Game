using UnityEngine;

/// <summary>
/// Mouse look camera. Handles:
/// - Vertical rotation on camera
/// - Horizontal rotation on character body
/// - Reads ADS sensitivity scale if ADS component is present
/// - Reads WallRun tilt target if WallRun component is present
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float verticalClamp    = 85f;

    [Header("References")]
    [SerializeField] private Transform cameraHolder; // The Camera's parent transform
    [SerializeField] private Transform body;         // Rotates horizontally (usually the character root)

    [Header("Camera Tilt")]
    [SerializeField] private float tiltSpeed = 8f;

    private float _xRotation;  // Vertical look
    private float _tiltZ;

    // Optional modules read at runtime
    private ADS     _ads;
    private WallRun _wallRun;

    private void Awake()
    {
        _ads     = GetComponentInParent<ADS>()     ?? GetComponent<ADS>();
        _wallRun = GetComponentInParent<WallRun>() ?? GetComponent<WallRun>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    /// <summary>
    /// Called every Update by PlayerInputBridge.
    /// </summary>
    public void HandleMouseLook(Vector2 mouseDelta)
    {
        float sensitivityScale = _ads != null ? _ads.SensitivityScale : 1f;
        float sens = mouseSensitivity * sensitivityScale;

        _xRotation -= mouseDelta.y * sens;
        _xRotation  = Mathf.Clamp(_xRotation, -verticalClamp, verticalClamp);

        cameraHolder.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        body.Rotate(Vector3.up, mouseDelta.x * sens);

        // Wall run tilt
        float tiltTarget = _wallRun != null ? _wallRun.CameraTiltTarget : 0f;
        _tiltZ = Mathf.Lerp(_tiltZ, tiltTarget, tiltSpeed * Time.deltaTime);

        cameraHolder.localRotation = Quaternion.Euler(_xRotation, 0f, _tiltZ);
    }
}
