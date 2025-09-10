using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public Transform centerPoint;   // The center of the circle
    public float radius = 5f;       // Circle radius
    public float speed = 2f;        // Rotation speed
    public bool detection;

    private float angle = 0f;
    private bool isMoving = false;  // Movement starts only after trigger exit
    private float cooldownTime;
    public float timeToDetect;

    private void Start()
    {
        ResetTime();
    }
    public void ResetTime()
    {
        cooldownTime = timeToDetect;
    }

    void Update()
    {
        if (!isMoving) return;

        cooldownTime-= Time.deltaTime;

        if (cooldownTime < 0)
        {
            detection = true;
        }
        else
        {
            detection = false;
        }

        // Increase angle
        angle += speed * Time.deltaTime;

        // Calculate new position (anticlockwise)
        float x = Mathf.Cos(angle) * radius;
        float z = -Mathf.Sin(angle) * radius;

        Vector3 newPos = new Vector3(centerPoint.position.x + x,
                                     centerPoint.position.y,
                                     centerPoint.position.z + z);

        // Move object
        transform.position = newPos;

        // Face along circular path
        Vector3 direction = (newPos - centerPoint.position).normalized;
        Vector3 tangent = new Vector3(direction.z, 0, -direction.x); // anticlockwise tangent
        transform.rotation = Quaternion.LookRotation(tangent, Vector3.up);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Circle"))
        {
            // Calculate the starting angle from current position
            Vector3 offset = transform.position - centerPoint.position;
            angle = Mathf.Atan2(-offset.z, offset.x); // anticlockwise start
            isMoving = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StopPoint"))
        {
            if (detection) 
            {
                isMoving = false;
                Debug.Log("Stopped at stopPoint: " + other.name);
            }
        }
    }
}
