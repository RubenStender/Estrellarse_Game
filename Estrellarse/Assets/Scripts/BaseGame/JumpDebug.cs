using UnityEngine;

public class JumpDebug : MonoBehaviour
{
    private Jump _jump;
    private CharacterMotor _motor;

    private void Awake()
    {
        _jump = GetComponent<Jump>();
        _motor = GetComponent<CharacterMotor>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Space pressed | jump={_jump != null} | grounded={_motor.IsGrounded}");
            _jump?.OnJumpPressed();
        }
    }
}