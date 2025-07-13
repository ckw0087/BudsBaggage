using UnityEngine;

public class PlanePointer : MonoBehaviour
{
    [SerializeField] Transform planePosition;
    [SerializeField] Transform playerPosition;
    [SerializeField] SpriteRenderer planeRender;
    private float rightLimit = 20, leftLimit = 20;
    Transform arrowTransform;
    Rigidbody2D arrowRigidbody;

    void Start()
    {
        arrowTransform = GetComponent<Transform>();
        //arrowRigidbody = arrow.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Limits();
    }

    void Limits()
    {
        if (planeRender.isVisible)
        {
            Debug.Log("Object is visible");
            transform.rotation = Quaternion.identity;

        }
        else
        {
            Debug.Log("Object is no longer visible");

            // Rotate the camera every frame so it keeps looking at the target
            float angle = Mathf.Atan2(planePosition.position.y - transform.position.y, planePosition.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
    }
}
