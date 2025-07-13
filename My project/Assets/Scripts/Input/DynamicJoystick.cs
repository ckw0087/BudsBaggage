using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [HideInInspector] public Vector2 input = Vector2.zero;
    [HideInInspector] public bool isSprinting;

    [SerializeField] private RectTransform _joystickBackground;
    [SerializeField] private RectTransform _joystickHandle;
    [SerializeField] private float _joystickRadius = 60f;

    private bool isJoystickActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _joystickBackground.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isJoystickActive)
            return;

        CheckScreenTouch();
    }

    private void CheckScreenTouch()
    {
        isSprinting = Input.touchCount > 1;
    }

    private void ActivateJoystick(Vector2 screenPosition)
    {
        isJoystickActive = true;

        // Move joystick BG to finger position
        _joystickBackground.gameObject.SetActive(true);

        // Convert screen position to canvas position
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _joystickBackground.parent as RectTransform,
            screenPosition,
            null,
            out pos
        );
        _joystickBackground.anchoredPosition = pos;
        _joystickHandle.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Required for mobile touch
        if (!isJoystickActive)
        {
            ActivateJoystick(eventData.position);
        }
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystickBackground.gameObject.SetActive(false);
        _joystickHandle.anchoredPosition = Vector2.zero;
        input = Vector2.zero;
        isJoystickActive = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _joystickBackground, 
            eventData.position, 
            eventData.pressEventCamera, 
            out pos
        );
        pos = Vector2.ClampMagnitude(pos, _joystickRadius);
        _joystickHandle.anchoredPosition = pos;
        input = pos / _joystickRadius;
    }
}
