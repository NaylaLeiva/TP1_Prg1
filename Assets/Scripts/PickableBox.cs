using UnityEngine;

public class PickableBox : MonoBehaviour
{
    private Rigidbody rb;
    private Collider boxCollider;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public bool IsPickedUp { get; private set; } = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        // Guardar la posición y rotación inicial de la caja
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // Si la caja cae por debajo de Y = -10 y no la llevas agarrada, vuelve a su inicio
        if (!IsPickedUp && transform.position.y < -10f)
        {
            ResetPosition();
        }
    }

    public void PickUp(Transform holdPoint)
    {
        IsPickedUp = true;

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (boxCollider != null)
        {
            boxCollider.enabled = false; // Desactiva colisión para no volar
        }

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        IsPickedUp = false;
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }

    // Método para regresar la caja a su posición de inicio
    public void ResetPosition()
    {
        Drop(); // Por si estaba siendo llevada, la suelta adecuadamente

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}