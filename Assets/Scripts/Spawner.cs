using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    [SerializeField] private GameObject obstaclePrefab; // Prefab a instanciar
    [SerializeField] private float startDelay = 1f;       // Tiempo antes de la primera generación
    [SerializeField] private float repeatRate = 2f;       // Frecuencia de generación en segundos

    void Start()
    {
        // Invoca el método 'SpawnObstacle' de forma repetitiva
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab != null)
        {
            // Instancia el prefab en la posición y rotación actual del Spawner
            Instantiate(obstaclePrefab, transform.position, transform.rotation);
        }
    }
}