using UnityEngine;
using Estrellarse.Weapons;

public class PlayerInputBridge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform cameraHolder;

    private CharacterMotor _motor;
    private WalkRun _walkRun;
    private Jump _jump;
    private Dash _dash;
    private Slide _slide;
    private WallRun _wallRun;
    private Shooter _shooter;
    private Reloader _reloader;
    private ADS _ads;
    private Melee _melee;
    private PoleGrab _poleGrab;

    private void Awake()
    {
        _motor = GetComponent<CharacterMotor>();
        _walkRun = GetComponent<WalkRun>();
        _jump = GetComponent<Jump>();
        _dash = GetComponent<Dash>();
        _slide = GetComponent<Slide>();
        _wallRun = GetComponent<WallRun>();
        _shooter = GetComponent<Shooter>();
        _reloader = GetComponent<Reloader>();
        _ads = GetComponent<ADS>();
        _melee = GetComponent<Melee>();
        _poleGrab = GetComponent<PoleGrab>();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleCombatInput();
        HandleMouseLook();
    }

    private void HandleMovementInput()
    {
        if (_walkRun != null)
        {
            _walkRun.MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"),
                                             Input.GetAxisRaw("Vertical"));
            _walkRun.IsSprinting = Input.GetKey(KeyCode.LeftShift);
            _walkRun.IsWalking = Input.GetKey(KeyCode.LeftControl);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_poleGrab != null && _poleGrab.IsActive)
                _poleGrab.TryJumpOff();
            else if (_wallRun != null && _wallRun.IsWallRunning)
                _wallRun.TryWallJump();
            else if (_jump != null)
                _jump.OnJumpPressed();
        }

        if (_jump != null && Input.GetKeyUp(KeyCode.Space))
            _jump.OnJumpReleased();

        if (_dash != null && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Vector3 dir = Vector3.zero;
            if (_walkRun != null && _walkRun.MoveInput.sqrMagnitude > 0.01f)
            {
                dir = transform.right * _walkRun.MoveInput.x
                    + transform.forward * _walkRun.MoveInput.y;
            }
            else
            {
                dir = transform.forward;
            }
            _dash.TryDash(dir);
        }

        if (_slide != null)
        {
            if (Input.GetKeyDown(KeyCode.C)) _slide.TrySlide();
            if (Input.GetKeyUp(KeyCode.C)) _slide.CancelSlide();
        }
    }

    private void HandleCombatInput()
    {
        if (_ads != null)
            _ads.SetAiming(Input.GetMouseButton(1));

        if (_shooter != null && Input.GetMouseButton(0))
        {
            Vector3 origin = cameraHolder != null ? cameraHolder.position : transform.position;
            Vector3 dir = cameraHolder != null ? cameraHolder.forward : transform.forward;
            _shooter.TryShoot(origin, dir);
        }

        if (_reloader != null && Input.GetKeyDown(KeyCode.R))
            _reloader.TryReload();

        if (_melee != null && Input.GetKeyDown(KeyCode.V))
            _melee.TryMelee();
    }

    private void HandleMouseLook()
    {
        if (cameraController == null) return;

        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"),
                                         Input.GetAxisRaw("Mouse Y"));
        cameraController.HandleMouseLook(mouseDelta);
    }
}