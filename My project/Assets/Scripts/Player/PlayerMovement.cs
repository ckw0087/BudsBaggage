using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] DynamicJoystick joystick;
    [SerializeField] Transform luggage;
    [SerializeField, Range(0f, 10f)] private float moveSpeed = 5.0f;
    [SerializeField, Range(0f, 10f)] private float sprintMultiplier = 2.0f;
    [SerializeField, Range(0f, 10f)] private float weightMultiplier = 2.0f;
    [SerializeField] private float speedBoostMultiplier = 2.0f;
    [SerializeField] private float speedBoostDuration = 2.0f;

    private float originalMoveSpeed;
    private Coroutine speedBoostRoutine;

    public bool IsMoving { get; private set; }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        //Joystick input
        Vector2 dir = joystick.input;
        float speed = joystick.isSprinting ? moveSpeed * sprintMultiplier - AlterSpeed(moveSpeed) : moveSpeed - AlterSpeed(moveSpeed);
        Vector3 moveDir = new Vector3(dir.x, dir.y, 0).normalized;
        transform.position += speed* Time.deltaTime * moveDir;
        Debug.Log(dir);

        IsMoving = dir.magnitude > 0.01f;
        //Debug.Log(moveDir);
    }

    private float AlterSpeed(float sprintMultiplier)
    {
        float alterSpeedMultiplier = (luggage.childCount * weightMultiplier) / sprintMultiplier;
        Debug.Log(alterSpeedMultiplier);

        return alterSpeedMultiplier;
    }

    public void ActivateSpeedBoost()
    {
        if (speedBoostRoutine != null)
            StopCoroutine(speedBoostRoutine);

        speedBoostRoutine = StartCoroutine(SpeedBoostRoutine(speedBoostMultiplier, speedBoostDuration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        moveSpeed = originalMoveSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed;
    }
}
