using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float edgePanThreshold = 0.05f; // Screen edge percentage for panning

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float zoomSmoothing = 5f;

    private float targetZoom;
    private Vector3 dragOrigin;

    void Start()
    {
        targetZoom = Camera.main.orthographicSize;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    void HandleMovement()
    {
        // WASD Movement
        Vector3 moveInput = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0
        ).normalized;

        // Edge Panning (optional)
        if (Input.mousePosition.x <= Screen.width * edgePanThreshold) moveInput.x = -1;
        if (Input.mousePosition.x >= Screen.width * (1 - edgePanThreshold)) moveInput.x = 1;
        if (Input.mousePosition.y <= Screen.height * edgePanThreshold) moveInput.y = -1;
        if (Input.mousePosition.y >= Screen.height * (1 - edgePanThreshold)) moveInput.y = 1;

        // Apply speed and sprint
        float currentSpeed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);
        transform.position += moveInput * currentSpeed * Time.deltaTime;
    }

    void HandleZoom()
    {
        // Q/E Zooming
        if (Input.GetKey(KeyCode.Q)) targetZoom += zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) targetZoom -= zoomSpeed * Time.deltaTime;

        // Mouse Wheel Zooming (optional)
        targetZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 5f;

        // Clamp and apply zoom
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        Camera.main.orthographicSize = Mathf.Lerp(
            Camera.main.orthographicSize,
            targetZoom,
            Time.deltaTime * zoomSmoothing
        );
    }
}