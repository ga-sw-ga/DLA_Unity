using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    
    public float radius = 10f; // Initial distance from the center (0,0,0)
    public float rotationSpeed = 100f; // Speed of camera rotation
    public float zoomSpeed = 2f; // Speed of radius change with scroll
    public float minRadius = 5f; // Minimum distance from the center
    public float maxRadius = 50f; // Maximum distance from the center

    private float angleX = 0f; // Horizontal rotation angle
    private float angleY = 0f; // Vertical rotation angle

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Initialize camera position
        UpdateCameraPosition();
    }

    void Update()
    {
        HandleMouseRotation();
        HandleZoom();
    }

    void HandleMouseRotation()
    {
        // Check for left mouse button press
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Adjust angles based on mouse movement
            angleX += mouseX * rotationSpeed * Time.deltaTime;
            angleY -= mouseY * rotationSpeed * Time.deltaTime;

            // Clamp vertical angle to prevent flipping
            angleY = Mathf.Clamp(angleY, -89f, 89f);

            // Update camera position
            UpdateCameraPosition();
        }
    }

    void HandleZoom()
    {
        // Get scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the radius based on scroll input
        radius -= scrollInput * zoomSpeed;

        // Clamp radius to specified limits
        radius = Mathf.Clamp(radius, minRadius, maxRadius);

        // Update camera position
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        // Convert spherical coordinates to Cartesian coordinates
        float radianX = Mathf.Deg2Rad * angleX;
        float radianY = Mathf.Deg2Rad * angleY;

        float x = radius * Mathf.Cos(radianY) * Mathf.Sin(radianX);
        float y = radius * Mathf.Sin(radianY);
        float z = radius * Mathf.Cos(radianY) * Mathf.Cos(radianX);

        // Set camera position
        transform.position = new Vector3(x, y, z);

        // Make the camera look at the center
        transform.LookAt(Vector3.zero);
    }
}
