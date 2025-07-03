using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    [Header("Shooting")]
    public float shootRange = 100f;
    public float shootRate = 0.2f;
    public LayerMask shootableLayers;
    public GameObject hitEffectPrefab;
    public GameObject projectilePrefab;
    public Transform shootPoint;

    [Header("Touch Points")]
    public List<Transform> hitPositions;
    public Gradient colorGradient;
    private TouchPoint[] touchPoints = { };

    public float maximumValue = 1.0f;
    public float valueDivider = 4f;
    public float valueDecreaseRateMultiplier = 2f;

    private float shootCooldown = 0f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public static Player singleton;

    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }

        touchPoints = new TouchPoint[hitPositions.Count];
        for (int i = 0; i < hitPositions.Count; i++)
        {
            touchPoints[i] = new TouchPoint();
        }

        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        HandleShooting();

        // Decay all touch values over time
        for (int i = 0; i < hitPositions.Count; i++)
        {
            if (touchPoints[i].value > 0)
            {
                touchPoints[i].value -= Time.deltaTime * valueDecreaseRateMultiplier;
                touchPoints[i].value = Mathf.Clamp(touchPoints[i].value, 0.0f, maximumValue);
            }
        }
    }

    public void TakeHit(Vector3 hit)
    {
        float minDist = float.MaxValue;
        int minI = -1;

        for (int i = 0; i < hitPositions.Count; i++)
        {
            float dist = Vector3.Distance(hit, hitPositions[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                minI = i;
            }
        }

        if (minI == -1)
        {
            Debug.LogError("TakeHit: Could not find closest point.");
            return;
        }

        ApplyHit(minI);
    }

    private void ApplyHit(int i)
    {
        touchPoints[i].value = maximumValue;
        print($"hit on {i}");
    }

    void OnDrawGizmos()
    {
        if (hitPositions == null || hitPositions.Count == 0)
            return;

        if (touchPoints.Length == 0)
            return;

        for (int i = 0; i < hitPositions.Count; i++)
        {
            var pos = hitPositions[i].position;
            var value = touchPoints != null ? touchPoints[i].value : 0f;

            Gizmos.color = colorGradient.Evaluate(value);
            Gizmos.DrawSphere(pos, 0.05f);
        }
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleShooting()
    {
        shootCooldown -= Time.deltaTime;

        if (Input.GetButton("Fire1") && shootCooldown <= 0f)
        {
            shootCooldown = shootRate;

            if (projectilePrefab != null && shootPoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
                Projectile p = projectile.GetComponent<Projectile>();
                p.thrownByPlayer = true;
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
    }
}

[System.Serializable]
public class TouchPoint
{
    public float value = 0f;
}