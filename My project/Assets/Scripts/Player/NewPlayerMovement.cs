using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private Transform luggage; // Parent for carried luggages
    [SerializeField, Range(1f, 20f)] private float moveSpeed = 10.0f;
    [SerializeField, Range(1f, 20f)] private float minMoveSpeed = 5.0f;
    [SerializeField, Range(0f, 10f)] private float sprintMultiplier = 2.0f;
    [SerializeField, Range(0f, 10f)] private float weightMultiplier = 2.0f;
    [SerializeField, Range(0f, 10f)] private float speedBoostMultiplier = 2.0f;
    [SerializeField] private float speedBoostDuration = 2.0f;

    private Rigidbody2D _rigidbody;

    private float _currentSpeedBoost = 1f;
    private Coroutine _speedBoostRoutine;
    public bool FacingRight { get; private set; } = true;

    public bool IsMoving { get; private set; }
    public Vector3 MoveDir { get; private set; }

    public event Action OnFlip;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector2 dir = joystick.input;
        MoveDir = new Vector3(dir.x, dir.y, 0).normalized;

        float speed = joystick.isSprinting
            ? moveSpeed * sprintMultiplier * _currentSpeedBoost - AlterSpeed(moveSpeed * sprintMultiplier * _currentSpeedBoost)
            : moveSpeed * _currentSpeedBoost - AlterSpeed(moveSpeed * _currentSpeedBoost);

        speed = Mathf.Max(minMoveSpeed, speed);

        _rigidbody.linearVelocity = MoveDir * moveSpeed;

        if (MoveDir != Vector3.zero)
        {
            if (FacingRight && MoveDir.x < 0)
            {
                FacingRight = false;
                OnFlip?.Invoke();
            }
            else if (!FacingRight && MoveDir.x > 0)
            {
                FacingRight = true;
                OnFlip?.Invoke();
            }
        }

        IsMoving = dir.magnitude > 0.01f;
    }

    private float AlterSpeed(float baseSpeed)
    {
        if (luggage == null) return 0f;
        float alterSpeedMultiplier = (luggage.childCount * weightMultiplier) / Mathf.Max(baseSpeed, 0.01f);
        return alterSpeedMultiplier;
    }

    // Called by powerup, or wherever needed
    public void ActivateSpeedBoost()
    {
        if (_speedBoostRoutine != null)
            StopCoroutine(_speedBoostRoutine);

        _speedBoostRoutine = StartCoroutine(SpeedBoostRoutine(speedBoostMultiplier, speedBoostDuration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        _currentSpeedBoost = multiplier;
        yield return new WaitForSeconds(duration);
        _currentSpeedBoost = 1f;
    }
}
