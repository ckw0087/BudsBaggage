using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] DynamicJoystick joystick;
    [SerializeField, Range(0f, 10f)] private float moveSpeed = 5.0f;

    private Vector2 startPosition;
    private Vector2 endPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Mobile
        HandleInput();

        #if UNITY_EDITOR
        //Editor fallback
        HandleInputEditor();
        #endif
    }

    private void HandleInput()
    {
        //Joystick input
        Vector2 dir = joystick.input;
        Vector3 moveDir = new Vector3(dir.x, dir.y, 0).normalized;
        transform.position += moveSpeed * Time.deltaTime * moveDir;
        Debug.Log(dir);
        //Debug.Log(moveDir);
    }

    #if UNITY_EDITOR
    private void HandleInputEditor()
    {
        //Usual editor input code
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, 0, y).normalized;
        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }
    #endif
}
