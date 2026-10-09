using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 4f;

    [Header("Power-Up / Potenciador (Consigna 5)")]
    [SerializeField] private float speedMultiplier = 1.8f; // Multiplicador de velocidad
    [SerializeField] private float powerUpDuration = 5f;   // Duración del efecto en segundos
    [SerializeField] private float cooldownDuration = 3f;  // Tiempo de recarga en segundos
    [SerializeField] private Renderer playerRenderer;      // Indicador visual
    [SerializeField] private Color activeColor = Color.yellow; // Color al estar activo
    [SerializeField] private Color cooldownColor = Color.gray; // Color durante recarga

    [Header("Configuración de Caída / Reinicio")]
    [SerializeField] private float fallThreshold = -10f;

    [Header("Configuración de Vista / Cámara")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Coleccionables")]
    [SerializeField] private int score = 0;

    [Header("Componentes")]
    [SerializeField] private Animator animator;

    [Header("Sistema de Agarre (Consigna 4)")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float pickupDistance = 4f;

    private PickableBox currentBox;

    private Rigidbody rb;
    private bool isGrounded;
    private float cameraXRotation = 0f;

    private float mouseX;
    private float mouseY;
    private float moveX;
    private float moveZ;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Transform activePlatform;
    private Vector3 lastPlatformPosition;

    // Variables internas del Power-Up (Consigna 5)
    private float currentPowerUpMultiplier = 1f;
    private bool isPowerUpActive = false;
    private bool isOnCooldown = false;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (playerRenderer == null)
        {
            playerRenderer = GetComponentInChildren<Renderer>();
        }

        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }
    }

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
            return;
        }

        // CORREGIDO: Adición del operador *
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        RotateCamera();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // --- CONSIGNA 4: TECLA 'E' PARA TOMAR O SOLTAR CAJA ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentBox == null)
            {
                TryPickUpBox();
            }
            else
            {
                DropBox();
            }
        }

        // --- CONSIGNA 5: ACTIVAR POWER-UP CON TECLA 'Q' ---
        if (Input.GetKeyDown(KeyCode.Q) && !isPowerUpActive && !isOnCooldown)
        {
            StartCoroutine(PowerUpRoutine());
        }
    }

    void FixedUpdate()
    {
        RotatePlayer();
        MovePlayer();
    }

    private void RotatePlayer()
    {
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * mouseX);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void RotateCamera()
    {
        if (cameraHolder != null)
        {
            cameraXRotation -= mouseY;
            cameraXRotation = Mathf.Clamp(cameraXRotation, minPitch, maxPitch);
            cameraHolder.localRotation = Quaternion.Euler(cameraXRotation, 0f, 0f);
        }
    }

    private void MovePlayer()
    {
        Vector3 moveInput = new Vector3(moveX, 0f, moveZ);
        bool isMoving = moveInput.magnitude > 0.1f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        float baseSpeed = isRunning ? runSpeed : walkSpeed;

        // CORREGIDO: Adición del operador *
        float currentSpeed = baseSpeed * currentPowerUpMultiplier;

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        Vector3 newVelocity = moveDirection * currentSpeed;

        Vector3 platformDelta = Vector3.zero;
        if (activePlatform != null)
        {
            platformDelta = (activePlatform.position - lastPlatformPosition) / Time.fixedDeltaTime;
            lastPlatformPosition = activePlatform.position;
        }

        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = new Vector3(newVelocity.x + platformDelta.x, rb.linearVelocity.y + platformDelta.y, newVelocity.z + platformDelta.z);
        }

        if (animator != null)
        {
            float speedParam = isMoving ? (isRunning ? 1f : 0.5f) : 0f;
            animator.SetFloat("Speed", speedParam);
            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    // --- CORRUTINA DEL POWER-UP (CONSIGNA 5) ---
    private IEnumerator PowerUpRoutine()
    {
        isPowerUpActive = true;
        currentPowerUpMultiplier = speedMultiplier;

        if (playerRenderer != null)
        {
            playerRenderer.material.color = activeColor;
        }

        yield return new WaitForSeconds(powerUpDuration);

        isPowerUpActive = false;
        isOnCooldown = true;
        currentPowerUpMultiplier = 1f;

        if (playerRenderer != null)
        {
            playerRenderer.material.color = cooldownColor;
        }

        yield return new WaitForSeconds(cooldownDuration);

        isOnCooldown = false;

        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor;
        }
    }

    private void Jump()
    {
        activePlatform = null;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    private void Respawn()
    {
        // Si tenías una caja agarrada, se resetea a su origen
        if (currentBox != null)
        {
            currentBox.ResetPosition();
            currentBox = null;
        }

        activePlatform = null;
        transform.position = startPosition;
        transform.rotation = startRotation;

        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void AddCollectible(int amount)
    {
        score += amount;
        Debug.Log("Puntos recolectados: " + score);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            activePlatform = collision.gameObject.transform;
            lastPlatformPosition = activePlatform.position;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("MovingPlatform") && activePlatform == null)
        {
            activePlatform = collision.gameObject.transform;
            lastPlatformPosition = activePlatform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            activePlatform = null;
        }
    }

    // --- LÓGICA DE DETECCIÓN MEJORADA PARA LA CAJA ---
    private void TryPickUpBox()
    {
        Transform rayOrigin = cameraHolder != null ? cameraHolder : transform;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        // 1. Raycast directo
        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            PickableBox box = hit.collider.GetComponentInParent<PickableBox>() ?? hit.collider.GetComponent<PickableBox>();
            if (box != null && !box.IsPickedUp)
            {
                currentBox = box;
                currentBox.PickUp(holdPoint);
                return;
            }
        }

        // 2. Esfera de detección por cercanía en frente del jugador (Garantiza el agarre)
        Collider[] colliders = Physics.OverlapSphere(rayOrigin.position + rayOrigin.forward * 1.5f, 1.5f);
        foreach (var col in colliders)
        {
            PickableBox box = col.GetComponentInParent<PickableBox>() ?? col.GetComponent<PickableBox>();
            if (box != null && !box.IsPickedUp)
            {
                currentBox = box;
                currentBox.PickUp(holdPoint);
                break;
            }
        }
    }

    private void DropBox()
    {
        if (currentBox != null)
        {
            currentBox.Drop();
            currentBox = null;
        }
    }
}