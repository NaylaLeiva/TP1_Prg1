using UnityEngine;
using TMPro; // Usar si usas TextMeshPro, o usa UnityEngine.UI para Text normal

public class DropZone : MonoBehaviour
{
    [Header("Configuración de Entrega (Consigna 6)")]
    [SerializeField] private Color winColor = Color.green;
    [SerializeField] private GameObject winTextUI; // Objeto de UI con el texto de victoria

    private bool hasWon = false;
    private Renderer zoneRenderer;

    private void Start()
    {
        zoneRenderer = GetComponent<Renderer>();

        if (winTextUI != null)
        {
            winTextUI.SetActive(false); // Oculta el mensaje al iniciar
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hasWon) return;

        // Detecta si entra un objeto con la clase PickableBox
        PickableBox box = other.GetComponentInParent<PickableBox>() ?? other.GetComponent<PickableBox>();

        if (box != null)
        {
            // La victoria se activa solo cuando el jugador SUELTA la caja dentro de la zona
            if (!box.IsPickedUp)
            {
                TriggerWin();
            }
        }
    }

    private void TriggerWin()
    {
        hasWon = true;
        Debug.Log("¡VICTORIA! Objeto entregado correctamente.");

        // 1. Mostrar mensaje de UI
        if (winTextUI != null)
        {
            winTextUI.SetActive(true);
        }

        // 2. Feedback visual en el escenario (Consigna 6)
        if (zoneRenderer != null)
        {
            zoneRenderer.material.color = winColor;
        }
    }
}