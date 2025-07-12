using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickInputForwarder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private DynamicJoystick _dynamicJoystick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _dynamicJoystick.OnPointerDown(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        _dynamicJoystick.OnDrag(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _dynamicJoystick.OnPointerUp(eventData);
    }
}
