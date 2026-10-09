using UnityEngine;

public class PlataformMovement : MonoBehaviour
{
    [SerializeField] private string axisMovement = "Y"; // Opciones: "X", "Y" o "Z"
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float waitTime = 1f;

    private Vector3 startPosition;
    private int direction = 1;
    private bool isMoving = true;
    private Rigidbody rb;

    void Start()
    {
        startPosition = transform.position;

        // Obtenemos o añadimos el Rigidbody automáticamente
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configuración necesaria para que la física transporte al personaje correctamente
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        if (isMoving)
        {
            CheckBoundaries(); // La verificación de límites se mantiene en Update
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            MovePlatform(); // El movimiento físico se ejecuta en FixedUpdate
        }
    }

    private void MovePlatform()
    {
        Vector3 displacement = Vector3.zero;
        string axis = axisMovement.ToUpper();

        if (axis == "X")
        {
            displacement = new Vector3(speed * direction * Time.fixedDeltaTime, 0f, 0f);
        }
        else if (axis == "Y")
        {
            displacement = new Vector3(0f, speed * direction * Time.fixedDeltaTime, 0f);
        }
        else if (axis == "Z")
        {
            displacement = new Vector3(0f, 0f, speed * direction * Time.fixedDeltaTime);
        }
        else
        {
            Debug.LogWarning("El eje '" + axisMovement + "' no es válido. Usa X, Y o Z en el Inspector.");
        }

        // Mover la plataforma usando la física del Rigidbody
        rb.MovePosition(rb.position + displacement);
    }

    private void CheckBoundaries()
    {
        bool positiveLimitReached = false;
        bool negativeLimitReached = false;

        string axis = axisMovement.ToUpper();

        if (axis == "X")
        {
            positiveLimitReached = (direction == 1 && transform.position.x >= startPosition.x + distance);
            negativeLimitReached = (direction == -1 && transform.position.x <= startPosition.x);
        }
        else if (axis == "Y")
        {
            positiveLimitReached = (direction == 1 && transform.position.y >= startPosition.y + distance);
            negativeLimitReached = (direction == -1 && transform.position.y <= startPosition.y);
        }
        else if (axis == "Z")
        {
            positiveLimitReached = (direction == 1 && transform.position.z >= startPosition.z + distance);
            negativeLimitReached = (direction == -1 && transform.position.z <= startPosition.z);
        }

        if (positiveLimitReached || negativeLimitReached)
        {
            PauseAndChangeDirection();
        }
    }

    private void PauseAndChangeDirection()
    {
        isMoving = false; // Detenemos el movimiento
        Invoke(nameof(ChangeDirection), waitTime); // Llama a ChangeDirection después de waitTime segundos
    }

    private void ChangeDirection()
    {
        direction *= -1; // Invierte el sentido
        isMoving = true;  // Reanuda el movimiento
    }
}