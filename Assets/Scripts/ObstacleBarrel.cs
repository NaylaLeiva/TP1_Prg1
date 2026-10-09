using UnityEngine;

public class ObstacleBarrel : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 1f; // Destruye el objeto tras N segundos

    void Start()
    {
        // Se autodestruye automáticamente para evitar acumulación
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Desplazamiento hacia adelante en el eje Z local
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}