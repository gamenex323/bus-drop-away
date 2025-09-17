using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameManager.ColorData vehicleColor;
    public int seatCapacity;
    public Transform centerPoint;   // The center of the circle
    public float radius = 5f;       // Circle radius
    public float speed = 2f;        // Rotation speed
    public bool detection;
    public MeshRenderer mesh;

    private float angle = 0f;
    private bool isMovingInCircle = false; 
    private bool isMovingForward = false; 
    private float cooldownTime;
    public float timeToDetect;
    public LayerMask vehicleLayer;  // assign "Vehicle" layer in Inspector

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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // Debug ray in Scene view

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, vehicleLayer))
            {
                Debug.Log("Raycast hit Vehicle: " + hit.collider.name);

                if (hit.transform == transform)
                {
                    Debug.Log("Tapped on vehicle player: " + gameObject.name);
                    isMovingForward = true;
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any Vehicle layer object!");
            }
        }

        if(isMovingForward)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (!isMovingInCircle) return;
        isMovingForward = false;

        cooldownTime -= Time.deltaTime;
        detection = cooldownTime < 0;

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
            // Calculate starting angle from current position
            Vector3 offset = transform.position - centerPoint.position;
            angle = Mathf.Atan2(-offset.z, offset.x); // anticlockwise start
            isMovingInCircle = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StopPoint"))
        {
            if (detection)
            {
                isMovingInCircle = false;
                isMovingForward = true;
                Debug.Log("Stopped at stopPoint: " + other.name);
            }
        }
    }

    private void OnValidate()
    {
        try
        {
           mesh.material = GameManager.instance.GetColor(vehicleColor);
        }
        catch 
        {
            
        }
    }
}
