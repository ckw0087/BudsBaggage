using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] DynamicJoystick joystick;
    [SerializeField, Range(0f, 10f)] private float moveSpeed = 5.0f;
    [SerializeField, Range(0f, 10f)] private float sprintMultiplier = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        //Joystick input
        Vector2 dir = joystick.input;
        float speed = joystick.isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        Vector3 moveDir = new Vector3(dir.x, dir.y, 0).normalized;
        transform.position += speed * Time.deltaTime * moveDir;
        Debug.Log(dir);
        //Debug.Log(moveDir);
    }
}
