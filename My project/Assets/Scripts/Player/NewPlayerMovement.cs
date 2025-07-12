using DG.Tweening;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    [SerializeField] DynamicJoystick joystick;
    [SerializeField, Range(0f, 10f)] private float moveSpeed = 5.0f;

    private Rigidbody2D _rigidbody;

    private Vector2 startPosition;
    private Vector2 endPosition;

    public Vector3 MoveDir { get; private set; }

    public bool IsMoving { get; private set; }

    private bool _facingRight = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

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
        MoveDir = new Vector3(dir.x, dir.y, 0).normalized;
        _rigidbody.linearVelocity = MoveDir * moveSpeed;
        if (MoveDir != Vector3.zero)
        {
            if (_facingRight && MoveDir.x < 0)
            {
                transform.DORotate(Vector3.up * 180f, 0.2f);
                _facingRight = false;
            }
            else if (!_facingRight && MoveDir.x > 0)
            {
                transform.DORotate(Vector3.zero, 0.2f);
                _facingRight = true;
            }
        }

        IsMoving = dir.magnitude > 0.01f;
        //Debug.Log(moveDir);
    }

#if UNITY_EDITOR
    private void HandleInputEditor()
    {
        //Usual editor input code
        /*float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        MoveDir = new Vector3(x, 0, y).normalized;
        transform.position += moveSpeed * Time.deltaTime * MoveDir;*/
    }
#endif
}
