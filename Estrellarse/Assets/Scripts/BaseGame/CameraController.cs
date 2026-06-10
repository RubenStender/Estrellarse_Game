using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float verticalClamp = 85f;

    [Header("References")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform body;

    [Header("Camera Tilt")]
    [SerializeField] private float tiltSpeed = 8f;

    private float _xRotation;
    private float _tiltZ;

    private ADS _ads;
    private WallRun _wallRun;
    private Slide _slide;

    private void Awake()
    {
        _ads = GetComponentInParent<ADS>() ?? GetComponent<ADS>();
        _wallRun = GetComponentInParent<WallRun>() ?? GetComponent<WallRun>();
        _slide = GetComponentInParent<Slide>() ?? GetComponent<Slide>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void HandleMouseLook(Vector2 mouseDelta)
    {
        float sensitivityScale = _ads != null ? _ads.SensitivityScale : 1f;
        float sens = mouseSensitivity * sensitivityScale;

        _xRotation -= mouseDelta.y * sens;
        _xRotation = Mathf.Clamp(_xRotation, -verticalClamp, verticalClamp);

        body.Rotate(Vector3.up, mouseDelta.x * sens);

        float wallTilt = _wallRun != null ? _wallRun.CameraTiltTarget : 0f;
        float slideTilt = _slide != null ? _slide.CameraTiltTarget : 0f;
        _tiltZ = Mathf.Lerp(_tiltZ, wallTilt + slideTilt, tiltSpeed * Time.deltaTime);

        cameraHolder.localRotation = Quaternion.Euler(_xRotation, 0f, _tiltZ);
    }
}