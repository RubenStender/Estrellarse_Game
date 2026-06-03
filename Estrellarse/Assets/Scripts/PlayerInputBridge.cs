using UnityEngine;

/// <summary>
/// THE ONLY player-specific script. Reads Input.GetKey and routes to modules.
/// All modules are optional — if a component isn't on the GameObject, that
/// input just does nothing. Add/remove abilities by adding/removing components.
///
/// ENEMY NOTE: Enemies do NOT use this. They call module methods directly from AI scripts.
/// </summary>
public class PlayerInputBridge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform        cameraHolder;    // Used for shoot origin

    // --- Cached components (all optional) ---
    private CharacterMotor _motor;
    private WalkRun        _walkRun;
    private Jump           _jump;
    private Dash           _dash;
    private Slide          _slide;
    private WallRun        _wallRun;
    private Shooter        _shooter;
    private Reloader       _reloader;
    private ADS            _ads;
    private Melee          _melee;

    private void Awake()
    {
        _motor    = GetComponent<CharacterMotor>();
        _walkRun  = GetComponent<WalkRun>();
        _jump     = GetComponent<Jump>();
        _dash     = GetComponent<Dash>();
        _slide    = GetComponent<Slide>();
        _wallRun  = GetComponent<WallRun>();
        _shooter  = GetComponent<Shooter>();
        _reloader = GetComponent<Reloader>();
        _ads      = GetComponent<ADS>();
        _melee    = GetComponent<Melee>();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleCombatInput();
        HandleMouseLook();
    }

    // ─── Movement ──────────────────────────────────────────────────────────

    private void HandleMovementInput()
    {
        // Walk / Run
        if (_walkRun != null)
        {
            _walkRun.MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"),
                                             Input.GetAxisRaw("Vertical"));
            _walkRun.IsSprinting = Input.GetKey(KeyCode.LeftShift);
            _walkRun.IsWalking   = Input.GetKey(KeyCode.LeftAlt);
        }

        // Jump
        if (_jump != null)
        {
            if (Input.GetKeyDown(KeyCode.Space)) _jump.OnJumpPressed();
            if (Input.GetKeyUp(KeyCode.Space))   _jump.OnJumpReleased();
        }

        // Wall jump (same key, WallRun handles whether it's valid)
        if (_wallRun != null && Input.GetKeyDown(KeyCode.Space))
            _wallRun.TryWallJump();

        // Dash — uses move direction, or forward if idle
        if (_dash != null && Input.GetKeyDown(KeyCode.LeftControl))
        {
            Vector3 dir = Vector3.zero;
            if (_walkRun != null && _walkRun.MoveInput.sqrMagnitude > 0.01f)
            {
                dir = transform.right   * _walkRun.MoveInput.x
                    + transform.forward * _walkRun.MoveInput.y;
            }
            else
            {
                dir = transform.forward;
            }
            _dash.TryDash(dir);
        }

        // Slide
        if (_slide != null)
        {
            if (Input.GetKeyDown(KeyCode.C)) _slide.TrySlide();
            if (Input.GetKeyUp(KeyCode.C))   _slide.CancelSlide();
        }
    }

    // ─── Combat ────────────────────────────────────────────────────────────

    private void HandleCombatInput()
    {
        // ADS
        if (_ads != null)
            _ads.SetAiming(Input.GetMouseButton(1));

        // Shoot (hold for auto, single click for semi-auto — fireRate handles it)
        if (_shooter != null && Input.GetMouseButton(0))
        {
            Vector3 origin = cameraHolder != null ? cameraHolder.position : transform.position;
            Vector3 dir    = cameraHolder != null ? cameraHolder.forward  : transform.forward;
            _shooter.TryShoot(origin, dir);
        }

        // Reload
        if (_reloader != null && Input.GetKeyDown(KeyCode.R))
            _reloader.TryReload();

        // Melee
        if (_melee != null && Input.GetKeyDown(KeyCode.V))
            _melee.TryMelee();
    }

    // ─── Mouse Look ────────────────────────────────────────────────────────

    private void HandleMouseLook()
    {
        if (cameraController == null) return;

        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"),
                                         Input.GetAxisRaw("Mouse Y"));
        cameraController.HandleMouseLook(mouseDelta);
    }
}
