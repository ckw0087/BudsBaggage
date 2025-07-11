using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [HideInInspector] public Vector2 input = Vector2.zero;

    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickHandle;
    [SerializeField] private float joystickRadius = 60f;

    private Vector2 startPos;
    private bool isActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        joystickBackground.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        //CheckScreenTouch();
    }

    private void CheckScreenTouch()
    {
        if (!isActive)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && touch.position.y < Screen.height / 2)
                {
                    Debug.Log("Touched");
                    ActivateJoystick(touch.position);
                }
            }

            #if UNITY_EDITOR
            //Editor fallback
            if (Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height / 2)
            {
                ActivateJoystick(Input.mousePosition);
            }
            #endif
        }
    }

    private void ActivateJoystick(Vector2 screenPosition)
    {
        // Move joystick BG to finger position
        joystickBackground.gameObject.SetActive(true);

        // Convert screen position to canvas position
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground.parent as RectTransform,
            screenPosition,
            null,
            out pos
        );
        joystickBackground.anchoredPosition = pos;
        joystickHandle.anchoredPosition = Vector2.zero;
        isActive = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Required for mobile touch
        if (!isActive)
        {
            ActivateJoystick(eventData.position);
        }
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickBackground.gameObject.SetActive(false);
        joystickHandle.anchoredPosition = Vector2.zero;
        input = Vector2.zero;
        isActive = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground, 
            eventData.position, 
            eventData.pressEventCamera, 
            out pos
        );
        pos = Vector2.ClampMagnitude(pos, joystickRadius);
        joystickHandle.anchoredPosition = pos;
        input = pos / joystickRadius;
    }
}
