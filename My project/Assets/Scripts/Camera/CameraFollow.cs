using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveThreshold = 0.1f;
    [SerializeField, Range(0f, 5f)] private float lookDistance = 2.5f;
    [SerializeField, Range(0f, 5f)] private float smoothingSpeed = 2.5f;

    private Vector3 lastPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target != null)
            //Set last position
            lastPosition = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        //Determine change in player position
        Vector3 playerDelta = target.position - lastPosition;
        Vector3 lookAhead = Vector3.zero;

        //Only look ahead if player actually moved
        if (playerDelta.magnitude > moveThreshold)
        {
            lookAhead = playerDelta.normalized * lookDistance;
        }

        //Determine future position
        Vector3 futurePosition = target.position + lookAhead;
        futurePosition.z = transform.position.z;

        //Smoothing
        transform.position = Vector3.Lerp(transform.position, futurePosition, smoothingSpeed);

        //Set last position
        lastPosition = target.position;
    }
}
